using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.RabbitMQ;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.ViewModel
{
    public partial class AddAsignaturaViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _Nombre;

        [ObservableProperty]
        public string _Descripcion;

        [ObservableProperty]
        public string _Creditos;

        [ObservableProperty]
        public string _Horas;

        private readonly IAsignaturaApiProvider _asignaturaApiService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public AddAsignaturaViewModel(IAsignaturaApiProvider asignaturaApi, IRabbitMQProducer rabbitMQProducer)
        {
            _asignaturaApiService = asignaturaApi;
            _rabbitMQProducer = rabbitMQProducer;
            _Creditos = "6";
        }

        [RelayCommand]
        public async Task Guardar()
        {
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Descripcion) || 
                string.IsNullOrEmpty(Creditos) || string.IsNullOrEmpty(Horas))
            {
                MessageBox.Show(Constantes.ERROR_CAMPOSNULL);
                return;
            }

            if (!int.TryParse(Creditos, out int creditos))
            {
                MessageBox.Show(Constantes.ERROR_CAMPOSNUMERICO);
                return;
            }

            if (!int.TryParse(Horas, out int horas))
            {
                MessageBox.Show(Constantes.ERROR_CAMPOSNUMERICO);
                return;
            }

            var asignatura = new AsignaturaDTO
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                Creditos = creditos,
                Horas = horas,
                PorcentajeFaltas = 0,
                Faltas = 0,
                IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
            };

            try
            {
                await _asignaturaApiService.PostAsignatura(asignatura);

                var mensaje = new MensajeRabbit
                {
                    Tipo = "Evento",
                    Contenido = $"Asignatura creada: {asignatura.Nombre} (Creditos: {asignatura.Creditos})"
                };
                await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));

                MessageBox.Show(Constantes.MSG_PERFECT);

                App.Current.Services.GetService<MainViewModel>().SelectViewModelCommand.Execute(App.Current.Services.GetService<PrincipalViewModel>());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la asignatura: {ex.Message}");
            }
        }

        public override Task LoadAsync()
        {
            return base.LoadAsync();
        }
    }
}
