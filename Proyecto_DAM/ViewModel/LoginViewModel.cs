using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Proyecto_DAM.Service;
using Proyecto_DAM.Utils;
using static Proyecto_DAM.Models.ExportJsonModel;

namespace Proyecto_DAM.ViewModel
{
    public partial class LoginViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string _Email;

        [ObservableProperty]
        public string _Password;

        [ObservableProperty]
        private bool _RememberMe;

        private readonly IHttpsJsonClientProvider<UserDTO> _httpJsonProvider;
        private readonly IUserApiProvider _UsuarioService;
        private readonly IAspNetUserApiProvider _aspNetUserApiProvider;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly IAsignaturaApiProvider _asignaturaApiService;
        private readonly IEventoApiProvider _eventoApiService;
        private readonly IBienestarApiProvider _bienestarApiService;
        private readonly ILogrosApiProvider _logrosService;

        public LoginViewModel(IHttpsJsonClientProvider<UserDTO> httpJsonProvider, IUserApiProvider userApi, IRabbitMQProducer rabbitMQProducer, IAspNetUserApiProvider aspNetUserApiProvider, IAsignaturaApiProvider asignaturaApiProvider, IEventoApiProvider eventoApiProvider, IBienestarApiProvider bienestarApiService, ILogrosApiProvider logrosService)
        {
            if (Properties.Settings.Default.RememberMe)
            {
                Email = Properties.Settings.Default.SavedEmail;
                Password = Properties.Settings.Default.SavedPassword;
                RememberMe = true;
            }

            _httpJsonProvider = httpJsonProvider;
            _aspNetUserApiProvider = aspNetUserApiProvider;
            _UsuarioService = userApi;
            _rabbitMQProducer = rabbitMQProducer;
            _asignaturaApiService = asignaturaApiProvider;
            _eventoApiService = eventoApiProvider;
            _bienestarApiService = bienestarApiService;
            _logrosService = logrosService;

            //Email = Constantes.EMAIL_GESTOR;
            //Password = Constantes.PASSWORD;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Por favor, introduzca su email y contraseña.");
                return;
            }

            App.Current.Services.GetService<LoginDTO>().Email = Email;
            App.Current.Services.GetService<LoginDTO>().Password = Password;

            try
            {
                UserDTO user = await _httpJsonProvider.LoginPostAsync(Constantes.LOGIN_PATH, App.Current.Services.GetService<LoginDTO>());

                if (user != null && user.Result != null && !string.IsNullOrEmpty(user.Result.Token))
                {
                    if (RememberMe)
                    {
                        Properties.Settings.Default.SavedEmail = Email;
                        Properties.Settings.Default.SavedPassword = Password;
                        Properties.Settings.Default.RememberMe = true;
                    }
                    else
                    {
                        Properties.Settings.Default.SavedEmail = string.Empty;
                        Properties.Settings.Default.SavedPassword = string.Empty;
                        Properties.Settings.Default.RememberMe = false;
                    }
                    Properties.Settings.Default.Save();

                    // Guardar el token en el servicio de LoginDTO
                    App.Current.Services.GetService<LoginDTO>().Token = user.Result.Token;

                    // Guardar el Id del usuario en el servicio de LoginDTO
                    IEnumerable<UsuarioDTO> Usuarios = await _UsuarioService.GetUser();
                    var Users = await _aspNetUserApiProvider.GetUsers();

                    var usuario = Usuarios.FirstOrDefault(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));
                    var userOne = Users.FirstOrDefault(u => u.Email.Equals(Email, StringComparison.OrdinalIgnoreCase));

                    if (usuario != null && user != null)
                    {
                        App.Current.Services.GetService<LoginDTO>().Id = usuario.Id;
                        App.Current.Services.GetService<LoginDTO>().UserName = userOne.UserName;
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

        public override async Task LoadAsync()
        {
            InicializarDatosAsync();
        }

        public async Task InicializarDatosAsync()
        {
            // 1. Comprobar si el admin ya está creado
            var usuarios = await _UsuarioService.GetUser();
            if (!usuarios.Any(u => u.Email == Constantes.EMAIL_GESTOR))
            {
                var gestor = new RegistroDTO(
                    Constantes.USERNAME_GESTOR,
                    Constantes.USERNAME_GESTOR,
                    Constantes.EMAIL_GESTOR,
                    Constantes.PASSWORD,
                    Constantes.ROLE_REGISTRER_ADMIN
                );
                await _httpJsonProvider.RegisterPostAsync(Constantes.REGISTER_PATH, gestor);

                var admin = new RegistroDTO(
                    Constantes.USERNAME,
                    Constantes.USERNAME,
                    Constantes.EMAIL,
                    Constantes.PASSWORD,
                    Constantes.ROLE_REGISTRER_ADMIN
                );
                await _httpJsonProvider.RegisterPostAsync(Constantes.REGISTER_PATH, admin);
            }
            else
            {
                return;
            }

            var user = await _UsuarioService.GetUser();
            var usuario = user.FirstOrDefault(u => u.Email.Equals(Constantes.EMAIL_GESTOR, StringComparison.OrdinalIgnoreCase));
            var userAdmin = user.FirstOrDefault(u => u.Email.Equals(Constantes.EMAIL, StringComparison.OrdinalIgnoreCase));
            int userId = usuario.Id;

            // 2. Crear asignaturas de 2º DAM
            var asignaturas = Cursos.CursoDAM.ObtenerAsignaturasDAM2();
            foreach (var a in asignaturas)
            {
                var asignatura = new AsignaturaDTO
                {
                    Nombre = a.Nombre,
                    Descripcion = a.Descripcion,
                    Creditos = a.Creditos,
                    Horas = a.Horas,
                    PorcentajeFaltas = 0,
                    Faltas = 0,
                    IdUsuario = userId
                };
                await _asignaturaApiService.PostAsignatura(asignatura);
            }

            try
            {
                var asignaturasUsuario = await _asignaturaApiService.GetAsignaturaIdUserPrueba(userId);
                var asignaturaCreada1 = asignaturasUsuario.ElementAtOrDefault(0);
                var asignaturaCreada2 = asignaturasUsuario.ElementAtOrDefault(1);
                var tipos = new[] { "Nota", "Tarea", "Examen" };
                var estados = new[] { "Pendiente", "EnProceso", "Realizado" };
                int i = 1;
                int tipoNum1 = 0;
                int estadoNum1 = 0;
                int tipoNum2 = 2;
                int estadoNum2 = 2;

                // 3. Crear 5 eventos en una asignatura
                var fechas = new[]
                {
                        DateTime.Now.AddHours(12),
                        DateTime.Now.AddDays(3),
                        DateTime.Now.AddDays(4),
                        DateTime.Now.AddDays(6),
                        DateTime.Now.AddDays(7),
                        DateTime.Now.AddDays(10)
                };

                foreach (var fecha in fechas)
                {
                    var evento1 = new EventoDTO
                    {
                        Nombre = $"Evento de {asignaturaCreada1.Nombre} {i}",
                        Descripcion = "Evento generado automáticamente",
                        Porcentaje = 10,
                        Fecha = fecha,
                        IdAsignatura = asignaturaCreada1.Id,
                        IdUsuario = userId,
                        Tipo = tipos[tipoNum1],
                        Estado = estados[estadoNum1],
                        EmailEnviado = false
                    };
                    var evento2 = new EventoDTO
                    {
                        Nombre = $"Evento de {asignaturaCreada2.Nombre} {i}",
                        Descripcion = "Evento generado automáticamente",
                        Porcentaje = 10,
                        Fecha = fecha,
                        IdAsignatura = asignaturaCreada2.Id,
                        IdUsuario = userId,
                        Tipo = tipos[tipoNum2],
                        Estado = estados[estadoNum2],
                        EmailEnviado = false
                    };
                    await _eventoApiService.PostEvento(evento1);
                    await _eventoApiService.PostEvento(evento2);
                    if (tipoNum1 == 2 || estadoNum1 == 2 || estadoNum2 == 0 || tipoNum2 == 0)
                    {
                        tipoNum1 = 0;
                        estadoNum1 = 0;
                        tipoNum2 = 2;
                        estadoNum2 = 2;
                    }
                    else
                    {
                        tipoNum1++;
                        estadoNum1++;
                        tipoNum2--;
                        estadoNum2--;
                    }
                    i++;
                }

                // 4. Añadir tiempo de estudio
                var tiempoEstudio = new TiempoEstudioDTO
                {
                    AsignaturaID = asignaturaCreada1.Id,
                    UsuarioId = userId,
                    TiempoEstudiado = TimeSpan.FromMinutes(120),
                    Fecha = DateTime.Now
                };
                await _asignaturaApiService.PostTiempoEstudio(tiempoEstudio);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al procesar la asignatura");
            }

            // 5. Añadir bienestar
            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var fecha = DateTime.Now.AddDays(-i -1);

                var estadoAnimo = random.Next(1, 4);
                var nivelEstres = random.Next(1, 11);

                var bienestar = new BienestarDTO
                {
                    UsuarioId = userId,
                    Fecha = fecha,
                    EstadoDeAnimo = estadoAnimo switch
                    {
                        1 => "Feliz",
                        2 => "Neutral",
                        3 => "Triste",
                        _ => "Desconocido"
                    },
                    NivelDeEstres = nivelEstres,
                    Sugerencia = "Registro automático para prueba de gráfico"
                };

                await _bienestarApiService.PostBienestar(bienestar);
            }
            // 6. Añadir logros a admin para ranking
            int adminId = userAdmin.Id;
            var ahora = DateTime.Now;

            var logro = (new GamificacionDTO
            {
                UsuarioId = adminId,
                Fecha = ahora,
                TipoDeLogro = "Aprobados",
                Puntos = 30,
                Descripcion = "¡Puntos!"
            });
            await _logrosService.PostLogro(logro);

            logro = (new GamificacionDTO
            {
                UsuarioId = adminId,
                Fecha = ahora,
                TipoDeLogro = "Estudio",
                Puntos = 20,
                Descripcion = "¡Estudio!"
            });
            await _logrosService.PostLogro(logro);

            logro = (new GamificacionDTO
            {
                UsuarioId = adminId,
                Fecha = ahora,
                TipoDeLogro = "Especial",
                Puntos = 50,
                Descripcion = "¡Especial!"
            });
            await _logrosService.PostLogro(logro);
        }
    }
}
