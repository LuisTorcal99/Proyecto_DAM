using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Service;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Proyecto_DAM.ViewModel
{
    public partial class PomodoroViewModel : ViewModelBase
    {
        private string _minutosIntroducidos;
        public string MinutosIntroducidos
        {
            get => _minutosIntroducidos;
            set => SetProperty(ref _minutosIntroducidos, value);
        }

        private string _tiempoRestante;
        public string TiempoRestante
        {
            get => _tiempoRestante;
            set => SetProperty(ref _tiempoRestante, value);
        }

        private Brush _fondoColor;
        public Brush FondoColor
        {
            get => _fondoColor;
            set => SetProperty(ref _fondoColor, value);
        }

        private DispatcherTimer _temporizador;
        private TimeSpan _duracion;

        private TiempoEstudioDTO _tiempoEstudioActual;

        [ObservableProperty]
        public DateTime _UltimaFechaEstudio;

        [ObservableProperty]
        public string _TiempoEstudiadoActual;

        [ObservableProperty]
        private ObservableCollection<string> _Asignaturas = new();

        [ObservableProperty]
        private string _AsignaturaSeleccionada;

        private readonly IAsignaturaApiProvider _asignaturaApiService;

        public PomodoroViewModel(IAsignaturaApiProvider asignaturaApiService)
        {
            TiempoRestante = "00:00";
            FondoColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6F0F1"));
            _asignaturaApiService = asignaturaApiService;
        }

        public override async Task LoadAsync()
        {
            Asignaturas.Clear();
            try
            {
                var asignaturas = await _asignaturaApiService.GetAsignatura();

                foreach (var asignatura in asignaturas)
                {
                    Asignaturas.Add(asignatura.Nombre);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar asignaturas: {ex.Message}");

            }
        }

        [RelayCommand]
        public void Empezar()
        {
            if (int.TryParse(MinutosIntroducidos, out int minutos))
            {
                var colorPersonalizado = (Color)ColorConverter.ConvertFromString("#D4C3E6");
                IniciarTemporizador(minutos, colorPersonalizado);
            }
            else
            {
                TiempoRestante = "Min inválidos";
            }
        }

        private void Temporizador_Tick(object sender, EventArgs e)
        {
            if (_duracion.TotalSeconds > 0)
            {
                _duracion = _duracion.Subtract(TimeSpan.FromSeconds(1));
                TiempoRestante = _duracion.ToString(@"mm\:ss");
            }
            else
            {
                _temporizador.Stop();
                TiempoRestante = "¡Fin!";

                FondoColor = new SolidColorBrush(Color.FromRgb(255, 120, 120));
            }
        }

        private async void IniciarTemporizador(int minutos, Color fondo)
        {
            _temporizador?.Stop();
            _duracion = TimeSpan.FromMinutes(minutos);
            TiempoRestante = _duracion.ToString(@"mm\:ss");

            FondoColor = new SolidColorBrush(fondo);

            _temporizador = new DispatcherTimer();
            _temporizador.Interval = TimeSpan.FromSeconds(1);
            _temporizador.Tick += Temporizador_Tick;
            _temporizador.Start();

            var asignatura = (await _asignaturaApiService.GetAsignatura())
                                .FirstOrDefault(a => a.Nombre == AsignaturaSeleccionada);

            if (asignatura != null)
            {
                var tiempoExistente = await _asignaturaApiService.GetTiempoEstudio(asignatura.Id);

                if (tiempoExistente == null)
                {
                    _tiempoEstudioActual = new TiempoEstudioDTO
                    {
                        AsignaturaID = asignatura.Id,
                        UsuarioId = App.Current.Services.GetService<LoginDTO>().Id,
                        TiempoEstudiado = TimeSpan.FromMinutes(minutos),
                        Fecha = DateTime.Now
                    };

                    await _asignaturaApiService.PostTiempoEstudio(_tiempoEstudioActual);
                }
                else
                {
                    tiempoExistente.TiempoEstudiado = tiempoExistente.TiempoEstudiado.Add(TimeSpan.FromMinutes(minutos));
                    tiempoExistente.Fecha = DateTime.Now;
                    await _asignaturaApiService.PatchTiempoEstudio(tiempoExistente);
                }
            }
        }

        partial void OnAsignaturaSeleccionadaChanged(string value)
        {
            _ = CargarTiempoEstudioAsync(value);
        }

        private async Task CargarTiempoEstudioAsync(string nombreAsignatura)
        {
            var asignatura = (await _asignaturaApiService.GetAsignatura())
                .FirstOrDefault(a => a.Nombre == nombreAsignatura);

            if (asignatura != null)
            {
                _tiempoEstudioActual = await (_asignaturaApiService as AsignaturaApiService).GetTiempoEstudio(asignatura.Id)
                    ?? new TiempoEstudioDTO
                    {
                        AsignaturaID = asignatura.Id,
                        UsuarioId = App.Current.Services.GetService<LoginDTO>().Id,
                        TiempoEstudiado = TimeSpan.Zero
                    };

                int horas = _tiempoEstudioActual.TiempoEstudiado.Hours;
                int minutos = _tiempoEstudioActual.TiempoEstudiado.Minutes;

                if (minutos >= 60)
                {
                    horas += minutos / 60;
                    minutos = minutos % 60;
                }

                TiempoEstudiadoActual = $"{horas}h {minutos}min";
                UltimaFechaEstudio = _tiempoEstudioActual.Fecha;
            }
            else
            {
                TiempoEstudiadoActual = "0 min";
            }
        }


        [RelayCommand]
        public void Trabajo()
        {
            IniciarTemporizador(25, Colors.Orange);
        }

        [RelayCommand]
        public void Descanso()
        {
            IniciarTemporizador(5, Colors.SkyBlue);
        }

    }
}
