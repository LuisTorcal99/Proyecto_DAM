using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Service;
using Proyecto_DAM.View;

namespace Proyecto_DAM.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;
        private readonly IAsignaturaApiProvider _asignaturaService;
        private readonly IEventoApiProvider _eventoService;
        private readonly INotaApiProvider _notaService;

        public MainViewModel(LoginViewModel login, RegistroViewModel registro,
            PrincipalViewModel principal, EventosViewModel eventos,
            PomodoroViewModel pomodoro, IAsignaturaApiProvider asignaturaService, 
            IEventoApiProvider eventoService, INotaApiProvider notaService)
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


            _asignaturaService = asignaturaService;
            _eventoService = eventoService;
            _notaService = notaService;
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

        [RelayCommand]
        public void AddEvento()
        {
            var viewModel = App.Current.Services.GetService<AddEventoViewModel>();

            if (viewModel is null)
            {
                MessageBox.Show("No se pudo cargar el ViewModel.");
                return;
            }

            var view = new AddEventoView
            {
                DataContext = viewModel
            };

            view.ShowDialog();
        }

        [RelayCommand]
        public async void ExportarExcel()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = "resumen_estudio.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string rutaArchivo = saveFileDialog.FileName;
                var workbook = new XLWorkbook();

                // Obtener todas las asignaturas, eventos y notas
                var asignaturas = (await _asignaturaService.GetAsignatura()).ToList();
                var eventos = (await _eventoService.GetEvento()).ToList();
                var notas = (await _notaService.GetNota()).ToList();

                // Filtrar asignaturas del usuario actual
                var asignaturasFiltradas = asignaturas
                        .Where(a => a.IdUsuario.Equals(App.Current.Services.GetService<LoginDTO>().Id))
                        .ToList();

                // Empezamos en la primera hoja del libro de Excel
                var hoja = workbook.Worksheets.Add("Resumen Estudio");

                // Contador de fila inicial
                int fila = 1;

                // Iterar sobre todas las asignaturas filtradas
                foreach (var asignatura in asignaturasFiltradas)
                {
                    hoja.Cell(fila, 1).Value = "Asignatura:";
                    hoja.Cell(fila, 1).Style.Font.Bold = true;
                    hoja.Cell(fila, 1).Style.Font.FontSize = 14;
                    fila++;

                    // Escribir los datos de la asignatura
                    hoja.Cell(fila, 1).Value = "Nombre:";
                    hoja.Cell(fila, 2).Value = asignatura.Nombre;
                    fila++;

                    hoja.Cell(fila, 1).Value = "Descripción:";
                    hoja.Cell(fila, 2).Value = asignatura.Descripcion;
                    fila++;

                    hoja.Cell(fila, 1).Value = "Créditos/Horas:";
                    hoja.Cell(fila, 2).Value = asignatura.Creditos;
                    fila++;

                    // Dejar un espacio entre créditos y eventos
                    fila++;

                    // Obtener los eventos asociados a la asignatura
                    var eventosAsignatura = eventos.Where(e => e.IdAsignatura == asignatura.Id).ToList();

                    if (eventosAsignatura.Any())
                    {
                        // Escribir los encabezados para los eventos
                        hoja.Cell(fila, 1).Value = "Eventos:";
                        hoja.Cell(fila, 1).Style.Font.Bold = true;
                        hoja.Cell(fila, 1).Style.Font.FontSize = 14;
                        fila++;

                        hoja.Cell(fila, 1).Value = "Nombre";
                        hoja.Cell(fila, 2).Value = "Descripción";
                        hoja.Cell(fila, 3).Value = "Fecha";
                        hoja.Cell(fila, 4).Value = "Porcentaje";
                        hoja.Cell(fila, 5).Value = "Tipo";
                        hoja.Cell(fila, 6).Value = "Estado";
                        hoja.Cell(fila, 7).Value = "Nota";
                        fila++;

                        // Escribir los eventos de la asignatura
                        foreach (var evento in eventosAsignatura)
                        {
                            hoja.Cell(fila, 1).Value = evento.Nombre;
                            hoja.Cell(fila, 2).Value = evento.Descripcion;
                            hoja.Cell(fila, 3).Value = evento.Fecha.ToString("dd/MM/yyyy");
                            hoja.Cell(fila, 4).Value = evento.Porcentaje;
                            hoja.Cell(fila, 5).Value = evento.Tipo;
                            hoja.Cell(fila, 6).Value = evento.Estado;

                            // Buscar la nota asociada al evento
                            var notaEvento = notas.FirstOrDefault(n => n.IdEvento == evento.Id);
                            hoja.Cell(fila, 7).Value = notaEvento?.NotaValor.ToString() ?? "Sin nota";

                            fila++;
                        }
                    }
                    else
                    {
                        hoja.Cell(fila, 1).Value = "No hay eventos asociados";
                        fila++;
                    }

                    // Añadir un espacio entre asignaturas
                    fila++;
                }

                // Ajustar el tamaño de las columnas
                hoja.Columns().AdjustToContents();

                // Guardar el archivo
                workbook.SaveAs(rutaArchivo);
                MessageBox.Show("Datos exportados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        [RelayCommand]
        public void ExportarPDF()
        {

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
