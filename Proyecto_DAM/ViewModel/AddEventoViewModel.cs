using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Models;
using Proyecto_DAM.RabbitMQ;  // Asegúrate de que la clase IRabbitMQProducer esté aquí
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.ViewModel
{
    public partial class AddEventoViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _Nombre;

        [ObservableProperty]
        public string _Descripcion;

        [ObservableProperty]
        public string _TipoSeleccionado;

        [ObservableProperty]
        public DateTime? _Fecha;

        [ObservableProperty]
        public string _HoraTexto;

        [ObservableProperty]
        public ObservableCollection<AsignaturaItemModel> _Asignaturas;

        [ObservableProperty]
        public AsignaturaItemModel _AsignaturaSeleccionada;
        public ObservableCollection<string> Tipos { get; set; }

        [ObservableProperty]
        public string _Porcentaje;

        private readonly IEventoApiProvider _eventoApiService;
        private readonly IRabbitMQProducer _rabbitMQProducer; 

        public AddEventoViewModel(IEventoApiProvider eventoApiProvider, IRabbitMQProducer rabbitMQProducer)
        {
            _eventoApiService = eventoApiProvider;
            _rabbitMQProducer = rabbitMQProducer; 
            Asignaturas = new ObservableCollection<AsignaturaItemModel>();
            Tipos = new ObservableCollection<string>() { "Tarea", "Examen" };
            _ = LoadAsync();
        }

        [RelayCommand]
        public async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Porcentaje) || string.IsNullOrWhiteSpace(AsignaturaSeleccionada.ToString())
                || Fecha == null || string.IsNullOrWhiteSpace(TipoSeleccionado) || string.IsNullOrWhiteSpace(HoraTexto))
            {
                MessageBox.Show(Constantes.ERROR_CAMPOSNULL);
                return;
            }

            if (!double.TryParse(Porcentaje, out double porcentaje))
            {
                MessageBox.Show("El porcentaje debe ser un valor numérico.");
                return;
            }

            if (porcentaje < 0 || porcentaje > 100)
            {
                MessageBox.Show("El porcentaje debe estar entre 0 y 100.");
                return;
            }

            DateTime fechaHoraFinal;
            try
            {
                var hora = DateTime.ParseExact(HoraTexto, "HH:mm", null);
                fechaHoraFinal = new DateTime(Fecha.Value.Year, Fecha.Value.Month, Fecha.Value.Day, hora.Hour, hora.Minute, 0);
            }
            catch (FormatException)
            {
                MessageBox.Show("La hora debe estar en el formato HH:mm.");
                return;
            }

            // Crear el evento
            var evento = new EventoDTO
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                Porcentaje = porcentaje,
                Fecha = fechaHoraFinal,
                IdAsignatura = AsignaturaSeleccionada.Id,
                IdUsuario = App.Current.Services.GetService<LoginDTO>().Id,
                Tipo = TipoSeleccionado,
                Estado = "Pendiente",
                EmailEnviado = false
            };

            try
            {
                await _eventoApiService.PostEvento(evento);
                MessageBox.Show(Constantes.MSG_PERFECT);

                string mensaje = $"Evento creado: {evento.Nombre} (Tipo: {evento.Tipo}, Asignatura: {evento.IdAsignatura}, Fecha: {evento.Fecha})";
                _rabbitMQProducer.EnviarMensaje(mensaje);

                MessageBox.Show("Evento enviado a RabbitMQ.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el evento: {ex.Message}");
            }
        }

        public override async Task LoadAsync()
        {
            try
            {
                var asignaturas = await App.Current.Services.GetService<IAsignaturaApiProvider>().GetAsignatura();

                var idUsuario = App.Current.Services.GetService<LoginDTO>().Id;

                var asignaturasUsuario = asignaturas
                    .Where(a => a.IdUsuario == idUsuario)
                    .Select(AsignaturaItemModel.CreateModelFromDTO)
                    .ToList();

                Asignaturas.Clear();
                foreach (var a in asignaturasUsuario)
                {
                    Asignaturas.Add(a);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaturas: {ex.Message}");
            }
        }
    }
}
