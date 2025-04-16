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
using Proyecto_DAM.Service;
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
        public ObservableCollection<AsignaturaItemModel> _Asignaturas;

        [ObservableProperty]
        public AsignaturaItemModel _AsignaturaSeleccionada;
        public ObservableCollection<string> Tipos { get; set; }

        [ObservableProperty]
        public string _Porcentaje;

        public IEventoApiProvider _eventoApiService;
        public AddEventoViewModel(IEventoApiProvider eventoApiProvider)
        {
            _eventoApiService = eventoApiProvider;
            Asignaturas = new ObservableCollection<AsignaturaItemModel>();
            Tipos = new ObservableCollection<string>() { "Tarea", "Examen" };
            _ = LoadAsync();
        }

        [RelayCommand]
        public async Task Guardar()
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Descripcion) ||
                string.IsNullOrWhiteSpace(Porcentaje) || Fecha == null)
            {
                MessageBox.Show(Constantes.ERROR_CAMPOSNULL);
                return;
            }

            // Validar Porcentaje como número entero
            if (!int.TryParse(Porcentaje, out int porcentaje))
            {
                MessageBox.Show("El porcentaje debe ser un valor numérico.");
                return;
            }

            // Crear el evento
            var evento = new EventoDTO
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                Porcentaje = porcentaje,
                Fecha = Fecha.Value,
                IdAsignatura = AsignaturaSeleccionada.Id,
                IdUsuario = App.Current.Services.GetService<LoginDTO>().Id,
                Tipo = TipoSeleccionado,
                Estado = "Pendiente"
            };

            try
            {
                await _eventoApiService.PostEvento(evento);
                MessageBox.Show(Constantes.MSG_PERFECT);
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
