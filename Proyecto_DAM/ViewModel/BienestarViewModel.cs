using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.ViewModel
{
    public partial class BienestarViewModel : ViewModelBase
    {
        private readonly IBienestarApiProvider _bienestarApiService;

        [ObservableProperty]
        public SeriesCollection _Series;

        [ObservableProperty]
        public List<string> _XLabels;

        [ObservableProperty]
        public Func<double, string> _YFormatter;

        // Las propiedades que cuentan los estados de ánimo y niveles de estrés deben ser de tipo int
        [ObservableProperty]
        private int _EstadoAnimoFelizCount;

        [ObservableProperty]
        private int _EstadoAnimoNeutralCount;

        [ObservableProperty]
        private int _EstadoAnimoTristeCount;

        [ObservableProperty]
        private int _EstresBajoCount;

        [ObservableProperty]
        private int _EstresMedioCount;

        [ObservableProperty]
        private int _EstresAltoCount;

        // Propiedades para los mensajes formateados
        [ObservableProperty]
        private string _EstadoAnimoFeliz;

        [ObservableProperty]
        private string _EstadoAnimoNeutral;

        [ObservableProperty]
        private string _EstadoAnimoTriste;

        [ObservableProperty]
        private string _EstresBajo;

        [ObservableProperty]
        private string _EstresMedio;

        [ObservableProperty]
        private string _EstresAlto;

        // Propiedades de texto para mostrar el estado de ánimo y nivel de estrés
        [ObservableProperty]
        private string _EstadoAnimoTexto;

        [ObservableProperty]
        private string _NivelEstresTexto;

        [ObservableProperty]
        private int _EstadoAnimo;

        [ObservableProperty]
        private int _NivelEstres;

        [ObservableProperty]
        private string _Sugerencia;

        public List<int> EstresData { get; set; }

        public BienestarViewModel(IBienestarApiProvider bienestarApiService)
        {
            _bienestarApiService = bienestarApiService;
        }

        public override async Task LoadAsync()
        {
            var bienestarList = await _bienestarApiService.GetBienestar();

            var fechaLimite = DateTime.Now.AddDays(-30);
            var bienestarUltimos30Dias = bienestarList
                .Where(b => b.Fecha >= fechaLimite)
                .ToList();

            await ActualizarConteos_Grafico(bienestarUltimos30Dias);
        }


        private async Task ActualizarConteos_Grafico(List<BienestarDTO> bienestarUltimos30Dias)
        {
            EstadoAnimoFelizCount = 0;
            EstadoAnimoNeutralCount = 0;
            EstadoAnimoTristeCount = 0;
            EstresBajoCount = 0;
            EstresMedioCount = 0;
            EstresAltoCount = 0;

            BienestarDTO registroMasReciente = null;
            var estresDataTemp = new List<double>();

            foreach (var bienestar in bienestarUltimos30Dias)
            {
                // Contadores de estado de ánimo
                if (bienestar.EstadoDeAnimo == "Triste")
                    EstadoAnimoTristeCount++;
                else if (bienestar.EstadoDeAnimo == "Neutral")
                    EstadoAnimoNeutralCount++;
                else if (bienestar.EstadoDeAnimo == "Feliz")
                    EstadoAnimoFelizCount++;

                // Contadores de estrés
                if (bienestar.NivelDeEstres >= 1 && bienestar.NivelDeEstres <= 4)
                    EstresBajoCount++;
                else if (bienestar.NivelDeEstres >= 5 && bienestar.NivelDeEstres <= 7)
                    EstresMedioCount++;
                else if (bienestar.NivelDeEstres >= 8 && bienestar.NivelDeEstres <= 10)
                    EstresAltoCount++;

                estresDataTemp.Add(bienestar.NivelDeEstres);

                // Buscar el registro más reciente
                if (registroMasReciente == null || bienestar.Fecha > registroMasReciente.Fecha)
                    registroMasReciente = bienestar;
            }

            // Asignar mensajes formateados
            EstadoAnimoFeliz = $"Alta: {EstadoAnimoFelizCount}";
            EstadoAnimoNeutral = $"Neutral: {EstadoAnimoNeutralCount}";
            EstadoAnimoTriste = $"Triste: {EstadoAnimoTristeCount}";
            EstresBajo = $"Estrés bajo: {EstresBajoCount}";
            EstresMedio = $"Estrés medio: {EstresMedioCount}";
            EstresAlto = $"Estrés alto: {EstresAltoCount}";

            if (registroMasReciente != null)
            {
                Sugerencia = $"Sugerencia reciente: {registroMasReciente.Sugerencia}";
            }
            else
            {
                Sugerencia = "";
            }

            var hace30Dias = DateTime.Today.AddDays(-30);

            XLabels = bienestarUltimos30Dias
                .Where(b => b.Fecha >= hace30Dias) 
                .OrderBy(b => b.Fecha)
                .Select(b => b.Fecha.ToString("dd/MM"))
                .Distinct()
                .ToList();

            // Agrupar por fecha (formato "dd/MM")
            var agrupadoPorFecha = bienestarUltimos30Dias
                .Where(b => b.Fecha >= hace30Dias)
                .GroupBy(b => b.Fecha.ToString("dd/MM"))
                .ToDictionary(g => g.Key, g =>
                {
                    var promedioAnimo = g.Average(b => b.EstadoDeAnimo switch
                    {
                        "Triste" => 0,
                        "Neutral" => 5,
                        "Feliz" => 10,
                        _ => 0
                    });

                    var promedioEstres = g.Average(b => b.NivelDeEstres);
                    return (Animo: promedioAnimo, Estres: promedioEstres);
                });

            var estadoAnimoData = new List<double>();
            var estresData = new List<double>();

            foreach (var label in XLabels)
            {
                if (agrupadoPorFecha.TryGetValue(label, out var datos))
                {
                    estadoAnimoData.Add(datos.Animo);
                    estresData.Add(datos.Estres);
                }
                else
                {
                    estadoAnimoData.Add(0);
                    estresData.Add(0);
                }
            }


            YFormatter = value => value.ToString("N0");

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Estado de Ánimo",
                    Values = new ChartValues<double>(estadoAnimoData),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1CAE4")),
                    StrokeThickness = 2,
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 10,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0")
                },
                new LineSeries
                {
                    Title = "Estrés",
                    Values = new ChartValues<double>(estresData),
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0E351B")),
                    StrokeThickness = 2,
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 10,
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0")
                }
            };
        }

        [RelayCommand]
        public async Task GuardarBienestar()
        {
            if (EstadoAnimo == 0)
            {
                EstadoAnimo = 1 ;
            }
            if (NivelEstres == 0)
            {
                NivelEstres = 1;
            }

            Sugerencia = ObtenerSugerencia(EstadoAnimo, NivelEstres);

            var bienestar = new BienestarDTO
            {
                UsuarioId = App.Current.Services.GetService<LoginDTO>().Id,
                Fecha = DateTime.Now,
                EstadoDeAnimo = EstadoAnimo switch
                {
                    1 => "Feliz",
                    2 => "Neutral",
                    3 => "Triste",
                    _ => "Desconocido"
                },
                NivelDeEstres = NivelEstres,
                Sugerencia = Sugerencia 
            };

            await _bienestarApiService.PostBienestar(bienestar);

            await LoadAsync();

            MessageBox.Show("Estado actualizado");
        }


        private string ObtenerSugerencia(int estadoAnimo, int nivelEstres)
        {
            if (estadoAnimo == 3) // Triste
            {
                if (nivelEstres > 8)
                {
                    return "Es un momento difícil. Es recomendable hablar con alguien cercano, meditar o buscar apoyo profesional.";
                }
                else if (nivelEstres > 6)
                {
                    return "Tomarte un tiempo para ti mismo podría ayudarte. Relájate con una actividad que disfrutes, como leer o escuchar música.";
                }
                else if (nivelEstres > 4)
                {
                    return "Haz ejercicio o sal a caminar para despejarte y mejorar tu estado de ánimo.";
                }
                else
                {
                    return "Intenta escribir lo que sientes o buscar un espacio tranquilo para reflexionar. A veces es útil hablar con un amigo.";
                }
            }
            else if (estadoAnimo == 2) // Neutral
            {
                if (nivelEstres > 8)
                {
                    return "Es momento de hacer una pausa y reducir el estrés. Considera practicar respiración profunda o meditación.";
                }
                else if (nivelEstres > 6)
                {
                    return "Aprovecha tu energía para organizarte. Hacer una lista de tareas pendientes puede ayudarte a sentirte más controlado.";
                }
                else if (nivelEstres > 4)
                {
                    return "Realiza una actividad física ligera o una caminata para mejorar tu ánimo y reducir el estrés.";
                }
                else
                {
                    return "Tómate un descanso y haz algo que te guste, como leer o disfrutar de un pasatiempo.";
                }
            }
            else if (estadoAnimo == 1) // Feliz
            {
                if (nivelEstres > 8)
                {
                    return "A pesar de estar bien, es importante no sobrecargarte. Aprovecha el día para relajarte y descansar un poco.";
                }
                else if (nivelEstres > 6)
                {
                    return "Tu energía es alta. Aprovecha para hacer tareas productivas o incluso compartir tu felicidad con los demás.";
                }
                else if (nivelEstres > 4)
                {
                    return "Haz lo que te haga feliz, ya sea pasar tiempo con seres queridos o realizar proyectos personales.";
                }
                else
                {
                    return "Disfruta del momento y mantén esa actitud positiva. Podrías ayudar a otros a sentirse igual de bien.";
                }
            }

            return "No se pudo generar una sugerencia adecuada.";
        }
    }
}
