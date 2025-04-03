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

namespace Proyecto_DAM.ViewModel
{
    public partial class PrincipalViewModel : ViewModelBase
    {
        private readonly IAsignaturaApiProvider _asignaturasService;
        private IHttpsJsonClientProvider<AsignaturaDTO> _httpsJsonClientProvider;

        private int _currentPage = 1;
        private readonly int _itemsPerPage = 5;

        public PrincipalViewModel(IAsignaturaApiProvider asignaturasService)
        {
            _asignaturasService = asignaturasService;
            DatosGridItem = new ObservableCollection<AsignaturaItemModel>();
            PaginatedItems = new ObservableCollection<AsignaturaItemModel>();
        }

        [ObservableProperty]
        private ObservableCollection<AsignaturaItemModel> _DatosGridItem;

        [ObservableProperty]
        private ObservableCollection<AsignaturaItemModel> _PaginatedItems;


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
                var asignaturas = await _asignaturasService.GetAsignatura();

                if (asignaturas != null)
                {
                    foreach (var asignatura in asignaturas)
                    {
                        DatosGridItem.Add(AsignaturaItemModel.CreateModelFromDTO(asignatura));
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
