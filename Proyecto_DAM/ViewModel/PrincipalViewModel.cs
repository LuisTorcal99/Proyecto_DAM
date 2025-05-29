using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Models;
using Proyecto_DAM.RabbitMQ;
using Proyecto_DAM.Service;
using Proyecto_DAM.Utils;
using Proyecto_DAM.View;
using static Proyecto_DAM.Models.ExportJsonModel;

namespace Proyecto_DAM.ViewModel
{
    public partial class PrincipalViewModel : ViewModelBase
    {
        private readonly IAsignaturaApiProvider _asignaturaService;
        private readonly IEventoApiProvider _eventoService;
        private readonly INotaApiProvider _notaService;
        private readonly IHttpsJsonClientProvider<AsignaturaDTO> _httpService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly IEventoNotificacionProvider _eventoNotificacion;

        [ObservableProperty]
        private ObservableCollection<AsignaturaItemModel> _AsignaturaItem;

        [ObservableProperty]
        private string _NumeroAsignaturas;

        public PrincipalViewModel(IAsignaturaApiProvider asignaturaService, IHttpsJsonClientProvider<AsignaturaDTO> httpService,
                        IServiceProvider serviceProvider, IEventoApiProvider eventoApiProvider, INotaApiProvider notaApiProvider,
                        IRabbitMQProducer rabbitMQProducer, IEventoNotificacionProvider eventoNotificacion)
        {
            _asignaturaService = asignaturaService;
            _httpService = httpService;
            AsignaturaItem = new ObservableCollection<AsignaturaItemModel>();
            _serviceProvider = serviceProvider;
            _eventoService = eventoApiProvider;
            _notaService = notaApiProvider;
            _rabbitMQProducer = rabbitMQProducer;
            _eventoNotificacion = eventoNotificacion;

            AsignaturaItem = new ObservableCollection<AsignaturaItemModel>();
        }

        [RelayCommand]
        private async Task ItemClick(int Id)
        {
            var viewModel = _serviceProvider.GetRequiredService<DetallesAsignaturaViewModel>();
            await viewModel.SetIdAsignatura(Id);

            var view = new DetallesAsignaturaView { DataContext = viewModel };
            view.ShowDialog();
        }

        public override async Task LoadAsync()
        {
            AsignaturaItem.Clear();
            NumeroAsignaturas = $"Asignaturas(0):";
            try
            {
                var asignaturas = await _asignaturaService.GetAsignatura();
                var eventos = await _eventoService.GetEvento();
                var notas = await _notaService.GetNota();
                var idUsuario = App.Current.Services.GetService<LoginDTO>().Id;

                if (asignaturas?.Any() == true)
                {

                    await _eventoNotificacion.VerificarYEnviarCorreos(eventos.ToList(), notas.ToList(), idUsuario);

                    NumeroAsignaturas = $"Asignaturas({asignaturas.ToList().Count}):";

                    foreach (var dto in asignaturas)
                    {
                        var model = AsignaturaItemModel.CreateModelFromDTO(dto);

                        model.TotalEventos = eventos.Count(e => e.IdAsignatura == model.Id);
                        model.TotalNotas = notas.Count(n => n.IdAsignatura == model.Id && n.NotaValor > -1);

                        AsignaturaItem.Add(model);
                    }
                }

                else
                {
                    MessageBox.Show(Constantes.ERROR_NO_DATOS);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaturas: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task AddAsignatura()
        {
            var viewModel = App.Current.Services.GetService<AddAsignaturaViewModel>();

            if (viewModel is null)
            {
                MessageBox.Show("No se pudo cargar el ViewModel.");
                return;
            }

            var view = new AddAsignaturaView
            {
                DataContext = viewModel
            };

            view.ShowDialog();
        }
    }
}