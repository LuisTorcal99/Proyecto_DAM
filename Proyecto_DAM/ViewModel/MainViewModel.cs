using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Service;
using Proyecto_DAM.View;
using Proyecto_DAM.ViewModel;

namespace Proyecto_DAM.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(LoginViewModel login, RegistroViewModel registro, PrincipalViewModel principal, EventosViewModel eventos)
        {
            SelectedViewModel = login;

            LoginViewModel = login;
            RegistroViewModel = registro;
            PrincipalViewModel = principal;
            EventosViewModel = eventos;
        }

        public LoginViewModel LoginViewModel { get; set; }
        public RegistroViewModel RegistroViewModel { get; set; }
        public PrincipalViewModel PrincipalViewModel { get; set; }
        public EventosViewModel EventosViewModel { get; set; }

        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                SetProperty(ref _selectedViewModel, value);
            }
        }

        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
            {
                await SelectedViewModel.LoadAsync();
            }
        }

        private bool _isMenuVisible = false; 

        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set => SetProperty(ref _isMenuVisible, value);
        }

        [RelayCommand]
        private async void SelectViewModel(object? parameter)
        {
            SelectedViewModel = parameter as ViewModelBase;
            await LoadAsync();

            // Mostrar menú en vistas seleccionadas
            IsMenuVisible = SelectedViewModel is PrincipalViewModel
                         || SelectedViewModel is EventosViewModel;
        }

        [RelayCommand]
        public void AddAsignatura()
        {
            var clientProvider = new HttpsJsonClientService<AsignaturaDTO>();
            var apiProvider = new AsignaturaApiService(clientProvider);
            var viewModel = new AddAsignaturaViewModel(apiProvider);

            var view = new AddAsignaturaView();
            view.DataContext = viewModel;
            view.ShowDialog();
        }

        [RelayCommand]
        public void AddEvento()
        {
            var clientProvider = new HttpsJsonClientService<EventoDTO>();
            var apiProvider = new EventoApiService(clientProvider);
            var viewModel = new AddEventoViewModel(apiProvider);

            var view = new AddEventoView();
            view.DataContext = viewModel;
            view.ShowDialog();
        }
    }
}
