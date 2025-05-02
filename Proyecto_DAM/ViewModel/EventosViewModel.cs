

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
        private readonly IAsignaturaApiProvider _AsignaturaApiService;

        private int _currentPage = 1;
        private readonly int _itemsPerPage = 10;

        public ObservableCollection<string> TiposEvento { get; } = new() {"Nota", "Tarea", "Examen" };
        public ObservableCollection<string> EstadosEvento { get; } = new() { "Pendiente", "EnProceso", "Completado" };

        [ObservableProperty]
        private ObservableCollection<string> _asignaturas = new();

        [ObservableProperty]
        private string _FiltroTipo;

        [ObservableProperty]
        private string _FiltroEstado;

        [ObservableProperty]
        private string _FiltroAsignatura;

        [ObservableProperty]
        private string _NumeroEventos;

        [ObservableProperty]
        private ObservableCollection<EventoItemModel> _DatosGridItem;

        [ObservableProperty]
        private ObservableCollection<EventoItemModel> _PaginatedItems;

        public EventosViewModel(IEventoApiProvider EventosService, IAsignaturaApiProvider asignaturaApiService)
        {
            _EventosService = EventosService;
            DatosGridItem = new ObservableCollection<EventoItemModel>();
            PaginatedItems = new ObservableCollection<EventoItemModel>();
            _AsignaturaApiService = asignaturaApiService;
        }

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
            Asignaturas.Clear();

            try
            {
                var eventos = await _EventosService.GetEvento();
                var asignaturas = await _AsignaturaApiService.GetAsignatura();

                if (eventos?.Any() == true)
                {
                    var eventosFiltrados = eventos
                        .Where(e => e.Fecha > DateTime.Now)
                        .Select(e => EventoItemModel.CreateModelFromDTO(e))
                        .OrderBy(e => e.Fecha)
                        .ToList();

                    NumeroEventos = $"Eventos({eventosFiltrados.Count}):";

                    foreach (var Asignatura in asignaturas)
                    {
                        Asignaturas.Add(Asignatura.Nombre);
                    }

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

        [RelayCommand]
        public void ResetearFiltros()
        {
            FiltroTipo = null;
            FiltroEstado = null;
            FiltroAsignatura = null;
            RefreshPaginatedItems(); 
        }

        [RelayCommand]
        public async Task AplicarFiltros()
        {
            var asignaturas = await _AsignaturaApiService.GetAsignatura();
            var asignaturaSeleccionada = asignaturas
                .FirstOrDefault(a => a.Nombre.Equals(FiltroAsignatura, StringComparison.OrdinalIgnoreCase))?.Id;


            var eventosFiltrados = DatosGridItem
                .Where(e =>
                    (string.IsNullOrEmpty(FiltroTipo) || e.Tipo.Contains(FiltroTipo, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(FiltroEstado) || e.Estado.Contains(FiltroEstado, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(FiltroAsignatura) || e.IdAsignatura == asignaturaSeleccionada)
                )
                .ToList();

            PaginatedItems.Clear();
            foreach (var evento in eventosFiltrados.Skip((CurrentPage - 1) * _itemsPerPage).Take(_itemsPerPage))
            {
                PaginatedItems.Add(evento);
            }
        }
    }
}
