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
            try
            {
                var asignaturas = await _asignaturaService.GetAsignatura();
                var eventos = await _eventoService.GetEvento();
                var notas = await _notaService.GetNota();
                var idUsuario = App.Current.Services.GetService<LoginDTO>().Id;

                if (asignaturas?.Any() == true)
                {

                    var asignaturasFiltradas = asignaturas
                        .Where(a => a.IdUsuario.Equals(App.Current.Services.GetService<LoginDTO>().Id))
                        .ToList();

                    var eventosFiltrados = eventos
                        .Where(e => e.IdUsuario.Equals(App.Current.Services.GetService<LoginDTO>().Id))
                        .ToList();

                    var notasFiltrados = notas
                        .Where(e => e.IdUsuario.Equals(App.Current.Services.GetService<LoginDTO>().Id))
                        .ToList();

                    await _eventoNotificacion.VerificarYEnviarCorreos(eventosFiltrados, notasFiltrados, idUsuario);

                    foreach (var dto in asignaturasFiltradas)
                    {
                        var model = AsignaturaItemModel.CreateModelFromDTO(dto);

                        model.TotalEventos = eventosFiltrados.Count(e => e.IdAsignatura == model.Id);
                        model.TotalNotas = notas.Count(n => n.IdAsignatura == model.Id && n.NotaValor > -1);

                        AsignaturaItem.Add(model);
                    }
                }

                else
                {
                    MessageBox.Show(Constantes.MSG_ERROR);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaturas: {ex.Message}");
            }
        }
    }
}