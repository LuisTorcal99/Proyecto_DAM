

using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Models;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.ViewModel
{
    public partial class EventosViewModel : ViewModelBase
    {
        private readonly IEventoApiProvider _EventosService;
        private IHttpsJsonClientProvider<EventoDTO> _httpsJsonClientProvider;

        private int _currentPage = 1;
        private readonly int _itemsPerPage = 5;

        public EventosViewModel(IEventoApiProvider EventosService)
        {
            _EventosService = EventosService;
            DatosGridItem = new ObservableCollection<EventoItemModel>();
            PaginatedItems = new ObservableCollection<EventoItemModel>();
        }

        [ObservableProperty]
        private ObservableCollection<EventoItemModel> _DatosGridItem;

        [ObservableProperty]
        private ObservableCollection<EventoItemModel> _PaginatedItems;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                RefreshPaginatedItems();
            }
        }

        public int PageCount => (DatosGridItem.Count + _itemsPerPage - 1) / _itemsPerPage;

        private void RefreshPaginatedItems()
        {
            PaginatedItems.Clear();
            var items = DatosGridItem
                .Skip((CurrentPage - 1) * _itemsPerPage)
                .Take(_itemsPerPage)
                .ToList();

            foreach (var item in items)
            {
                PaginatedItems.Add(item);
            }
        }

        [RelayCommand]
        public void MoveToNextPage()
        {
            if (CurrentPage < PageCount)
            {
                CurrentPage++;
            }
        }

        [RelayCommand]
        public void MoveToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        public override async Task LoadAsync()
        {
            DatosGridItem.Clear();
            try
            {
                var eventos = await _EventosService.GetEvento();

                if (eventos?.Any() == true)
                {
                    var idUsuario = App.Current.Services.GetService<LoginDTO>().Id;

                    var eventosFiltrados = eventos
                        .Where(e => e.IdUsuario == idUsuario && e.Fecha > DateTime.Now) 
                        .Select(e => EventoItemModel.CreateModelFromDTO(e))
                        .OrderBy(e => e.Fecha)
                        .ToList();

                    foreach (var evento in eventosFiltrados)
                    {
                        DatosGridItem.Add(evento);
                    }

                    RefreshPaginatedItems();
                }
                else
                {
                    MessageBox.Show(Constantes.MSG_ERROR);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
