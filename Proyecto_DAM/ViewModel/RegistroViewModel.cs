using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.RabbitMQ;
using Proyecto_DAM.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Proyecto_DAM.ViewModel
{
    public partial class RegistroViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _Username;

        [ObservableProperty]
        public string _Email;

        [ObservableProperty]
        public string _Password;

        [ObservableProperty]
        public string _ConfirmPassword;

        private readonly IHttpsJsonClientProvider<UserDTO> _httpJsonProvider;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public RegistroViewModel(IHttpsJsonClientProvider<UserDTO> httpJsonProvider, IRabbitMQProducer rabbitMQProducer)
        {
            _httpJsonProvider = httpJsonProvider;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [RelayCommand]
        private async Task Registro()
        {
            if (string.IsNullOrEmpty(Email)
                || string.IsNullOrEmpty(Password)
                || string.IsNullOrEmpty(ConfirmPassword)
                || string.IsNullOrEmpty(Username))
            {
                MessageBox.Show(Constantes.ERROR_CAMPOSNULL);
                return;
            }
            if (!IsValidEmail(Email))
            {
                MessageBox.Show(Constantes.CORREO_NO_VALIDO);
                return;
            }
            if (!ComprobacionPassword(Password, ConfirmPassword))
            {
                return;
            }
            try
            {
                RegistroDTO UsuarioRegistrado = new RegistroDTO(
                    Username, Username, Email, Password, Constantes.ROLE_REGISTRER_ADMIN
                );

                UserDTO user = await _httpJsonProvider.RegisterPostAsync(Constantes.REGISTER_PATH, UsuarioRegistrado);

                var mensaje = new MensajeRabbit
                {
                    Tipo = "Evento",
                    Contenido = $"Nuevo usuario registrado: {Username} (Email: {Email})"
                };
                await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));

                MessageBox.Show(Constantes.REGISTRO_EXITOSO);

                // Cambiar la vista a Login
                App.Current.Services.GetService<MainViewModel>().SelectedViewModel = App.Current.Services.GetService<MainViewModel>().LoginViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [RelayCommand]
        private void Login()
        {
            var mainViewModel = App.Current.Services.GetService<MainViewModel>();
            var LoginViewModel = App.Current.Services.GetService<LoginViewModel>();

            mainViewModel.SelectViewModelCommand.Execute(LoginViewModel);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = Constantes.PATTERN_CORREO;
            return Regex.IsMatch(email, pattern);
        }

        private bool ComprobacionPassword(string firstPass, string secondPass)
        {
            if (!firstPass.Equals(secondPass))
            {
                MessageBox.Show(Constantes.ERROR_PASSWORDEQUALS);
                return false;
            }
            if (firstPass.Length < 8 || firstPass.Length > 20)
            {
                MessageBox.Show(Constantes.CARACTERES_MASMENOS);
                return false;
            }
            if (!firstPass.Any(char.IsDigit))
            {
                MessageBox.Show(Constantes.LETRA_NUMERO);
                return false;
            }
            if (!firstPass.Any(char.IsLower))
            {
                MessageBox.Show(Constantes.LETRA_MINUSCULA);
                return false;
            }
            if (!firstPass.Any(char.IsUpper))
            {
                MessageBox.Show(Constantes.LETRA_MAYUSCULA);
                return false;
            }
            var symbols = Constantes.SIMBOLOS_PASSWORD;
            if (!firstPass.Any(c => symbols.Contains(c)))
            {
                MessageBox.Show(Constantes.LETRA_SIMBOLO);
                return false;
            }
            return true;
        }

        public override Task LoadAsync()
        {
            return base.LoadAsync();
        }
    }
}
