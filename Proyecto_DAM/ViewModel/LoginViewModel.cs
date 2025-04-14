using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;
using Microsoft.Extensions.DependencyInjection;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public LoginViewModel(IHttpsJsonClientProvider<UserDTO> httpJsonProvider, IUserApiProvider userApi)
        {
            _httpJsonProvider = httpJsonProvider;
            _UsuarioService = userApi;
            CrearAdmin();
            Email = Constantes.EMAIL;
            Password = Constantes.PASSWORD;
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

        private async void CrearAdmin()
        {
            var admin = new RegistroDTO(
                // name, username, email, password, role
                Constantes.USERNAME,
                Constantes.USERNAME,
                Constantes.EMAIL,
                Constantes.PASSWORD,
                Constantes.ROLE_REGISTRER_ADMIN
            );
            await _httpJsonProvider.RegisterPostAsync(Constantes.REGISTER_PATH, admin);
        }

        public override Task LoadAsync()
        {
            return base.LoadAsync();
        }
    }
}
