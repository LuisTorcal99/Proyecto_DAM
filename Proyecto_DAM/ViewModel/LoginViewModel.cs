using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.RabbitMQ;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.ViewModel
{
    public partial class LoginViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _Email;

        [ObservableProperty]
        public string _Password;

        private readonly IHttpsJsonClientProvider<UserDTO> _httpJsonProvider;
        private readonly IUserApiProvider _UsuarioService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public LoginViewModel(IHttpsJsonClientProvider<UserDTO> httpJsonProvider, IUserApiProvider userApi, IRabbitMQProducer rabbitMQProducer)
        {
            _httpJsonProvider = httpJsonProvider;
            _UsuarioService = userApi;
            _rabbitMQProducer = rabbitMQProducer;
            //CrearAdmin();
            //Email = Constantes.EMAIL;
            //Password = Constantes.PASSWORD;
        }

        [RelayCommand]
        private async Task Login()
        {
            App.Current.Services.GetService<LoginDTO>().Email = Email;
            App.Current.Services.GetService<LoginDTO>().Password = Password;

            try
            {
                UserDTO user = await _httpJsonProvider.LoginPostAsync(Constantes.LOGIN_PATH, App.Current.Services.GetService<LoginDTO>());

                if (user != null && user.Result != null && !string.IsNullOrEmpty(user.Result.Token))
                {
                    // Guardar el token en el servicio de LoginDTO
                    App.Current.Services.GetService<LoginDTO>().Token = user.Result.Token;

                    // Guardar el Id del usuario en el servicio de LoginDTO
                    IEnumerable<UsuarioDTO> Users = await _UsuarioService.GetUser();

                    var usuario = Users.FirstOrDefault(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));

                    if (usuario != null)
                    {
                        App.Current.Services.GetService<LoginDTO>().Id = usuario.Id;
                    }

                    // Enviar mensaje a RabbitMQ
                    var mensaje = new MensajeRabbit
                    {
                        Tipo = "Evento",
                        Contenido = $"Login exitoso para el usuario: {usuario?.Email}"
                    };
                    await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));

                    // Cambiar de vista
                    var mainViewModel = App.Current.Services.GetService<MainViewModel>();
                    var inicioViewModel = App.Current.Services.GetService<PrincipalViewModel>();
                    mainViewModel.SelectViewModelCommand.Execute(inicioViewModel);
                }
                else
                {
                    MessageBox.Show(Constantes.USER_PASSWORD_NOT_GOOD);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [RelayCommand]
        private void Registro()
        {
            var mainViewModel = App.Current.Services.GetService<MainViewModel>();
            var RegistroViewModel = App.Current.Services.GetService<RegistroViewModel>();

            mainViewModel.SelectViewModelCommand.Execute(RegistroViewModel);
        }

        //private async void CrearAdmin()
        //{
        //    var admin = new RegistroDTO(
        //        // name, username, email, password, role
        //        Constantes.USERNAME,
        //        Constantes.USERNAME,
        //        Constantes.EMAIL,
        //        Constantes.PASSWORD,
        //        Constantes.ROLE_REGISTRER_ADMIN
        //    );
        //    await _httpJsonProvider.RegisterPostAsync(Constantes.REGISTER_PATH, admin);
        //}

        public override Task LoadAsync()
        {
            return base.LoadAsync();
        }
    }
}
