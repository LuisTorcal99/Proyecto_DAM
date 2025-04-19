using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
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

        public PomodoroViewModel()
        {
            TiempoRestante = "00:00";
            FondoColor = new SolidColorBrush(Colors.White);
        }

        public override Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        public void Empezar()
        {
            FondoColor = new SolidColorBrush(Colors.White);
            if (int.TryParse(MinutosIntroducidos, out int minutos))
            {
                _duracion = TimeSpan.FromMinutes(minutos);
                TiempoRestante = _duracion.ToString(@"mm\:ss");

                _temporizador = new DispatcherTimer();
                _temporizador.Interval = TimeSpan.FromSeconds(1);
                _temporizador.Tick += Temporizador_Tick;
                _temporizador.Start();
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

                FondoColor = new SolidColorBrush(Colors.Red); 
            }
        }

        private void IniciarTemporizador(int minutos, Color fondo)
        {
            _temporizador?.Stop(); 
            _duracion = TimeSpan.FromMinutes(minutos);
            TiempoRestante = _duracion.ToString(@"mm\:ss");

            FondoColor = new SolidColorBrush(fondo);

            _temporizador = new DispatcherTimer();
            _temporizador.Interval = TimeSpan.FromSeconds(1);
            _temporizador.Tick += Temporizador_Tick;
            _temporizador.Start();
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
