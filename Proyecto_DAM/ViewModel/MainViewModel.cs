using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Service;
using Proyecto_DAM.View;

namespace Proyecto_DAM.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;

        public MainViewModel(LoginViewModel login, RegistroViewModel registro,
            PrincipalViewModel principal, EventosViewModel eventos,
            PomodoroViewModel pomodoro)
        {
            LoginViewModel = login;
            RegistroViewModel = registro;
            PrincipalViewModel = principal;
            EventosViewModel = eventos;
            PomodoroViewModel = pomodoro;

            SelectedViewModel = login;
            IsMenuVisible = false;

            var settings = SettingsManager.LoadSettings();
            SelectedTheme = settings.SelectedTheme;
        }

        public LoginViewModel LoginViewModel { get; }
        public RegistroViewModel RegistroViewModel { get; }
        public PrincipalViewModel PrincipalViewModel { get; }
        public EventosViewModel EventosViewModel { get; }
        public PomodoroViewModel PomodoroViewModel { get; }

        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                SetProperty(ref _selectedViewModel, value);
                IsMenuVisible = value is PrincipalViewModel || value is EventosViewModel || value is PomodoroViewModel;
            }
        }

        public async override Task LoadAsync()
        {
            if (SelectedViewModel is not null)
                await SelectedViewModel.LoadAsync();
        }

        private bool _isMenuVisible;

        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set => SetProperty(ref _isMenuVisible, value);
        }

        // ----- Comandos -----

        [RelayCommand]
        private async void SelectViewModel(object? parameter)
        {
            if (parameter is ViewModelBase viewModel)
            {
                SelectedViewModel = viewModel;
                await LoadAsync();
            }
        }

        [RelayCommand]
        public void AddAsignatura()
        {
            var apiProvider = new AsignaturaApiService(new HttpsJsonClientService<AsignaturaDTO>());
            var viewModel = new AddAsignaturaViewModel(apiProvider);
            var view = new AddAsignaturaView { DataContext = viewModel };
            view.ShowDialog();
        }

        [RelayCommand]
        public void AddEvento()
        {
            var apiProvider = new EventoApiService(new HttpsJsonClientService<EventoDTO>());
            var viewModel = new AddEventoViewModel(apiProvider);
            var view = new AddEventoView { DataContext = viewModel };
            view.ShowDialog();
        }

        // ----- Tema -----

        // Lista con los temas
        public List<string> Themes { get; } = new() { "Claro", "Oscuro" };

        // Propiedad seleccionada para el tema
        private string _selectedTheme;
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged();
                    ApplyTheme(_selectedTheme);  // Llamamos a ApplyTheme para cambiar el tema
                    SaveThemePreference();  // Guardar el tema seleccionado
                }
            }
        }

        // Guardar el tema seleccionado en el archivo de configuración
        private void SaveThemePreference()
        {
            var settings = new SettingsManager { SelectedTheme = _selectedTheme };
            settings.SaveSettings();  // Guardar en el archivo
        }

        // Método para cambiar los colores del tema
        private void ApplyTheme(string theme)
        {
            var resources = Application.Current.Resources;

            if (theme == "Claro")
            {
                resources["BackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1CAE4"));
                resources["TextColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#34495E"));
            }
            else if (theme == "Oscuro")
            {
                resources["BackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0E351B"));
                resources["TextColor"] = new SolidColorBrush(Colors.White);
            }
        }
    }
}
