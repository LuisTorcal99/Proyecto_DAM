using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.ViewModel
{
    public partial class BienestarViewModel : ViewModelBase
    {
        private readonly IBienestarApiProvider _bienestarApiService;

        public BienestarViewModel(IBienestarApiProvider bienestarApiService)
        {
            _bienestarApiService = bienestarApiService;
        }

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

        public override async Task LoadAsync()
        {
            var bienestarList = await _bienestarApiService.GetBienestar();

            var fechaLimite = DateTime.Now.AddDays(-30);
            var bienestarUltimos30Dias = bienestarList
                .Where(b => b.Fecha >= fechaLimite)
                .ToList();

            ActualizarConteos(bienestarUltimos30Dias);
        }

        private void ActualizarConteos(List<BienestarDTO> bienestarUltimos30Dias)
        {
            EstadoAnimoFelizCount = 0;
            EstadoAnimoNeutralCount = 0;
            EstadoAnimoTristeCount = 0;
            EstresBajoCount = 0;
            EstresMedioCount = 0;
            EstresAltoCount = 0;

            BienestarDTO registroMasReciente = null;

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

            Sugerencia = $"Sugerencia reciente: {registroMasReciente.Sugerencia}";
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
