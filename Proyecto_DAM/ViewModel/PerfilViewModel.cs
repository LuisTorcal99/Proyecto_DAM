using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.RabbitMQ;
using static Proyecto_DAM.Models.ExportJsonModel;

namespace Proyecto_DAM.ViewModel
{
    public partial class PerfilViewModel : ViewModelBase
    {
        private readonly IActualizarPerfilProvider _actualizarPerfilService;
        private readonly IAsignaturaApiProvider _asignaturaApiService;
        private readonly INotaApiProvider _notaApiService;
        private readonly IEventoApiProvider _eventoApiService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        [ObservableProperty]
        public SeriesCollection _Series;

        [ObservableProperty]
        public List<string> _XLabels; 

        [ObservableProperty]
        public Func<double, string> _YFormatter; 

        [ObservableProperty]
        public string _Name;

        [ObservableProperty]
        public string _UserName;

        [ObservableProperty]
        public string _Email;

        [ObservableProperty]
        private int _TotalAsignaturas;

        [ObservableProperty]
        private int _TotalEventos;

        [ObservableProperty]
        private int _TotalExamenes;

        [ObservableProperty]
        private int _TotalAprobadas;

        [ObservableProperty]
        private int _TotalSuspendidas;

        [ObservableProperty]
        private int _TotalPendientes;

        [ObservableProperty]
        private int _TotalExamenesAprobados;

        [ObservableProperty]
        private int _TotalExamenesSuspendidos;

        [ObservableProperty]
        private int _TotalExamenesPendientes;

        public PerfilViewModel(IActualizarPerfilProvider actualizarPerfilProvider,
                                IAsignaturaApiProvider asignaturaApiProvider,
                                IEventoApiProvider eventoApiProvider,
                                INotaApiProvider notaApiProvider,
                                IRabbitMQProducer rabbitMQProducer)
        {
            _actualizarPerfilService = actualizarPerfilProvider;
            _asignaturaApiService = asignaturaApiProvider;
            _eventoApiService = eventoApiProvider;
            _notaApiService = notaApiProvider;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [RelayCommand]
        public async Task Editar()
        {
            var loginDto = App.Current.Services.GetService<LoginDTO>();
            if (loginDto == null)
                return;

            var aspNetUser = await _actualizarPerfilService.ObtenerUserAspNetPorId(loginDto.Id);

            if (aspNetUser == null)
            {
                Console.WriteLine("No se pudieron obtener los datos del usuario.");
                return;
            }

            bool datosModificados = false;

            if ( aspNetUser.Name != Name)
            {
                aspNetUser.Name = Name;
                datosModificados = true;
            }

            if (aspNetUser.Email != Email)
            {
                aspNetUser.Email = Email;
                datosModificados = true;
            }

            if (aspNetUser.UserName != UserName)
            {
                aspNetUser.UserName = UserName;
                datosModificados = true;
            }

            if (!datosModificados)
            {
                MessageBox.Show("No se han realizado cambios.");
                return;
            }

            bool actualizado = await _actualizarPerfilService.ActualizarAspNetUser(aspNetUser);

            if (actualizado)
            {
                MessageBox.Show("Perfil actualizado correctamente.");
                var mensaje = new MensajeRabbit
                {
                    Tipo = "Evento",
                    Contenido = $"Perfil actualizado con exito"
                };
                await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
            }
            else
            {
                MessageBox.Show("Hubo un problema al actualizar el perfil.");
            }
        }

        public override async Task LoadAsync()
        {
            var loginDto = App.Current.Services.GetService<LoginDTO>();
            if (loginDto == null)
                return;

            var aspNetUser = await _actualizarPerfilService.ObtenerUserAspNetPorId(loginDto.Id);

            if (aspNetUser != null)
            {
                Name = aspNetUser.Name;
                Email = aspNetUser.Email;
                UserName = aspNetUser.UserName;
            }

            try
            {
                var asignaturas = await _asignaturaApiService.GetAsignatura();
                TotalAsignaturas = asignaturas?.ToList().Count ?? 0;

                var eventos = await _eventoApiService.GetEvento();
                TotalEventos = eventos?.ToList().Count ?? 0;

                // Filtrar los eventos de tipo "Examen"
                var eventosExamen = eventos?.Where(e => e.Tipo == "Examen").ToList();

                var notas = await _notaApiService.GetNota();

                // Aprobadas
                TotalAprobadas = notas?.Count(n => n.NotaValor >= 5) ?? 0;
                TotalExamenesAprobados = notas?.Count(n => n.NotaValor >= 5 && eventosExamen.Any(e => e.Id == n.IdEvento)) ?? 0;

                // Suspendidas
                TotalSuspendidas = notas?.Count(n => n.NotaValor >= 0 && n.NotaValor < 5) ?? 0;
                TotalExamenesSuspendidos = notas?.Count(n => n.NotaValor >= 0 && n.NotaValor < 5 && eventosExamen.Any(e => e.Id == n.IdEvento)) ?? 0;

                // Pendientes
                TotalPendientes = TotalEventos - (TotalAprobadas + TotalSuspendidas);
                TotalExamenesPendientes = eventosExamen.Count() - (TotalExamenesAprobados + TotalExamenesSuspendidos);

                if (TotalPendientes < 0) TotalPendientes = 0;
                if (TotalExamenesPendientes < 0) TotalExamenesPendientes = 0;


                XLabels = new List<string> { "Aprobadas", "Suspendidas", "Pendientes" };

                Series = new SeriesCollection
                {
                    // Columnas para las Notas Generales
                    new ColumnSeries
                    {
                        Title = "Notas",
                        Values = new ChartValues<double> { TotalAprobadas, TotalSuspendidas, TotalPendientes },
                        Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1CAE4")),
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        MaxColumnWidth = 60,
                        DataLabels = true,
                        LabelPoint = point => point.Y.ToString("N0")
                    },
                    // Columnas para las Notas de Examen
                    new ColumnSeries
                    {
                        Title = "Notas Examen",
                        Values = new ChartValues<double> { TotalExamenesAprobados, TotalExamenesSuspendidos, TotalExamenesPendientes },
                        Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0E351B")),
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        MaxColumnWidth = 60,
                        DataLabels = true,
                        LabelPoint = point => point.Y.ToString("N0")
                    }
                };

                YFormatter = value => value.ToString("N0");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos del perfil: {ex.Message}");
            }
        }
    }
}