using System.IO;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Service;
using Proyecto_DAM.Utils;
using Proyecto_DAM.View;
using static Proyecto_DAM.Models.ExportJsonModel;

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

        public async Task<(List<AsignaturaDTO>, List<EventoDTO>, List<NotaDTO>)> ObtenerAsignaturasEventosNotasAsync()
        {
            // Obtener todas las asignaturas, eventos y notas
            var asignaturas = (await _asignaturaService.GetAsignatura()).ToList();
            var eventos = (await _eventoService.GetEvento()).ToList();
            var notas = (await _notaService.GetNota()).ToList();

            // Filtrar asignaturas del usuario actual
            var asignaturasFiltradas = asignaturas
                .Where(a => a.IdUsuario.Equals(App.Current.Services.GetService<LoginDTO>().Id))
                .ToList();

            return (asignaturasFiltradas, eventos, notas);
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
                Filter = Constantes.EXCEL_FILTER,
                FileName = "resumen_estudio.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string rutaArchivo = saveFileDialog.FileName;
                var workbook = new XLWorkbook();

                var (asignaturasFiltradas, eventos, notas) = await ObtenerAsignaturasEventosNotasAsync();

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
                    hoja.Cell(fila, 1).Style.Font.Bold = true;
                    hoja.Cell(fila, 2).Value = asignatura.Nombre;
                    fila++;

                    hoja.Cell(fila, 1).Value = "Descripción:";
                    hoja.Cell(fila, 1).Style.Font.Bold = true;
                    hoja.Cell(fila, 2).Value = asignatura.Descripcion;
                    fila++;

                    hoja.Cell(fila, 1).Value = "Créditos/Horas:";
                    hoja.Cell(fila, 1).Style.Font.Bold = true;
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
                        hoja.Cell(fila, 1).Style.Font.Bold = true;
                        hoja.Cell(fila, 2).Value = "Descripción";
                        hoja.Cell(fila, 2).Style.Font.Bold = true;
                        hoja.Cell(fila, 3).Value = "Fecha";
                        hoja.Cell(fila, 3).Style.Font.Bold = true;
                        hoja.Cell(fila, 4).Value = "Porcentaje";
                        hoja.Cell(fila, 4).Style.Font.Bold = true;
                        hoja.Cell(fila, 5).Value = "Tipo";
                        hoja.Cell(fila, 5).Style.Font.Bold = true;
                        hoja.Cell(fila, 6).Value = "Estado";
                        hoja.Cell(fila, 6).Style.Font.Bold = true;
                        hoja.Cell(fila, 7).Value = "Nota";
                        hoja.Cell(fila, 7).Style.Font.Bold = true;
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
        public async void ExportarPDF()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = Constantes.PDF_FILTER,
                FileName = "resumen_estudio.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string rutaArchivo = saveFileDialog.FileName;

                // Crear un documento PDF
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Definir fuentes
                XFont font = new XFont("Arial", 10);
                XFont fontBlack = new XFont("Arial Black", 10);

                // Espacios iniciales
                double x = 40;
                double y = 40;
                double lineHeight = 18;

                var (asignaturasFiltradas, eventos, notas) = await ObtenerAsignaturasEventosNotasAsync();

                // Iterar sobre todas las asignaturas filtradas
                foreach (var asignatura in asignaturasFiltradas)
                {
                    // Título de la asignatura
                    gfx.DrawString("Asignatura:", fontBlack, XBrushes.Black, x, y);
                    y += lineHeight;

                    gfx.DrawString($"Nombre: {asignatura.Nombre}", font, XBrushes.Black, x, y);
                    y += lineHeight;
                    gfx.DrawString($"Descripción: {asignatura.Descripcion}", font, XBrushes.Black, x, y);
                    y += lineHeight;
                    gfx.DrawString($"Créditos: {asignatura.Creditos}", font, XBrushes.Black, x, y);
                    y += lineHeight;

                    y += lineHeight;

                    // Obtener los eventos asociados a la asignatura
                    var eventosAsignatura = eventos.Where(e => e.IdAsignatura == asignatura.Id).ToList();

                    if (eventosAsignatura.Any())
                    {
                        // Encabezado de la tabla de eventos
                        gfx.DrawString("Eventos:", fontBlack, XBrushes.Black, x, y);
                        y += lineHeight;

                        // Ajustar el ancho de las columnas
                        double columnWidth1 = 100;  // Espacio para "Nombre"
                        double columnWidth2 = 120;  // Espacio para "Descripción"
                        double columnWidth3 = 80;   // Espacio para "Fecha"
                        double columnWidth4 = 80;   // Espacio para "Porcentaje"
                        double columnWidth5 = 60;   // Espacio para "Tipo"
                        double columnWidth6 = 60;   // Espacio para "Estado"
                        double columnWidth7 = 60;   // Espacio para "Nota"

                        gfx.DrawString("Nombre", font, XBrushes.Black, x, y);
                        gfx.DrawString("Descripción", font, XBrushes.Black, x + columnWidth1, y);
                        gfx.DrawString("Fecha", font, XBrushes.Black, x + columnWidth1 + columnWidth2, y);
                        gfx.DrawString("Porcentaje", font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3, y);
                        gfx.DrawString("Tipo", font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4, y);
                        gfx.DrawString("Estado", font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5, y);
                        gfx.DrawString("Nota", font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5 + columnWidth6, y);
                        y += lineHeight;
                        // Subrayar los títulos
                        gfx.DrawLine(XPens.Black, x, y - lineHeight + 5, x + columnWidth1, y - lineHeight + 5); // Nombre
                        gfx.DrawLine(XPens.Black, x + columnWidth1, y - lineHeight + 5, x + columnWidth1 + columnWidth2, y - lineHeight + 5); // Descripción
                        gfx.DrawLine(XPens.Black, x + columnWidth1 + columnWidth2, y - lineHeight + 5, x + columnWidth1 + columnWidth2 + columnWidth3, y - lineHeight + 5); // Fecha
                        gfx.DrawLine(XPens.Black, x + columnWidth1 + columnWidth2 + columnWidth3, y - lineHeight + 5, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4, y - lineHeight + 5); // Porcentaje
                        gfx.DrawLine(XPens.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4, y - lineHeight + 5, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5, y - lineHeight + 5); // Tipo
                        gfx.DrawLine(XPens.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5, y - lineHeight + 5, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5 + columnWidth6, y - lineHeight + 5); // Estado
                        gfx.DrawLine(XPens.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5 + columnWidth6, y - lineHeight + 5, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5 + columnWidth6 + 60, y - lineHeight + 5); // Nota

                        // Añadir las filas de eventos
                        foreach (var evento in eventosAsignatura)
                        {
                            gfx.DrawString(evento.Nombre, font, XBrushes.Black, x, y);
                            gfx.DrawString(evento.Descripcion, font, XBrushes.Black, x + columnWidth1, y);
                            gfx.DrawString(evento.Fecha.ToString("dd/MM/yyyy"), font, XBrushes.Black, x + columnWidth1 + columnWidth2, y);
                            gfx.DrawString(evento.Porcentaje.ToString(), font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3, y);
                            gfx.DrawString(evento.Tipo, font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4, y);
                            gfx.DrawString(evento.Estado, font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5, y);

                            // Buscar la nota asociada al evento
                            var notaEvento = notas.FirstOrDefault(n => n.IdEvento == evento.Id);
                            gfx.DrawString(notaEvento?.NotaValor.ToString() ?? "Sin nota", font, XBrushes.Black, x + columnWidth1 + columnWidth2 + columnWidth3 + columnWidth4 + columnWidth5 + columnWidth6, y);
                            y += lineHeight;

                            // Comprobar si necesitamos una nueva página
                            if (y > page.Height - 100) 
                            {
                                page = document.AddPage();
                                gfx = XGraphics.FromPdfPage(page);
                                y = 40; // Reiniciar la posición vertical
                            }
                        }

                    }
                    else
                    {
                        gfx.DrawString("No hay eventos asociados", font, XBrushes.Black, x, y);
                        y += lineHeight;
                    }

                    // Dejar un espacio entre asignaturas
                    y += lineHeight;

                    // Comprobar si necesitamos una nueva página
                    if (y > page.Height - 100)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = 40; // Reiniciar la posición vertical
                    }
                }

                // Guardar el archivo PDF
                document.Save(rutaArchivo);
                MessageBox.Show("PDF exportado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        [RelayCommand]
        public async void ExportarJSON()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = Constantes.JSON_FILTER,
                FileName = "resumen_estudio.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string rutaArchivo = saveFileDialog.FileName;

                var (asignaturasFiltradas, eventos, notas) = await ObtenerAsignaturasEventosNotasAsync();

                // Crear lista de asignaturas para exportar
                var asignaturasExportar = new List<Asignatura>();

                foreach (var asignatura in asignaturasFiltradas)
                {
                    // Crear objeto Asignatura con eventos
                    var eventosAsignatura = eventos.Where(e => e.IdAsignatura == asignatura.Id).ToList();
                    var eventosExportar = eventosAsignatura.Select(e => new Evento
                    {
                        Nombre = e.Nombre,
                        Descripcion = e.Descripcion,
                        Fecha = e.Fecha,
                        Porcentaje = e.Porcentaje,
                        Tipo = e.Tipo,
                        Estado = e.Estado,
                        Nota = notas.FirstOrDefault(n => n.IdEvento == e.Id)?.NotaValor
                    }).ToList();

                    asignaturasExportar.Add(new Asignatura
                    {
                        Nombre = asignatura.Nombre,
                        Descripcion = asignatura.Descripcion,
                        Creditos = asignatura.Creditos,
                        Eventos = eventosExportar
                    });
                }

                // Crear instancia de FileService para exportar los datos a JSON
                var fileService = new FileService<Asignatura>();
                fileService.Save(rutaArchivo, asignaturasExportar);

                MessageBox.Show("JSON exportado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
                    ApplyTheme(_selectedTheme);  
                    SaveThemePreference();  
                }
            }
        }

        // Guardar el tema seleccionado en el archivo de configuración
        private void SaveThemePreference()
        {
            var settings = new SettingsManager { SelectedTheme = _selectedTheme };
            settings.SaveSettings();  
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
