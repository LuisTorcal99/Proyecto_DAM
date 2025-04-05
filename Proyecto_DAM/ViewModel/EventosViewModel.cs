

using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Models;

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
                var Eventos = await _EventosService.GetEvento();

                if (Eventos != null)
                {
                    foreach (var Evento in Eventos)
                    {
                        DatosGridItem.Add(EventoItemModel.CreateModelFromDTO(Evento));
                    }
                }
                RefreshPaginatedItems();
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
