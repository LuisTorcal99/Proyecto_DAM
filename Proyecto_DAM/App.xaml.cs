using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.RabbitMQ;
using Proyecto_DAM.Service;
using Proyecto_DAM.Services;
using Proyecto_DAM.Utils;
using Proyecto_DAM.ViewModel;

namespace Proyecto_DAM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await DockerHelper.StartDockerAsync();

            var consumidor = Services.GetService<IRabbitMQConsumer>();
            Task.Run(() => consumidor.IniciarConsumo());

            var mainWindow = Current.Services.GetService<MainWindow>();
            mainWindow?.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var consumidor = Services.GetService<IRabbitMQConsumer>();
            consumidor?.DetenerConsumo();

            await DockerHelper.StopDockerAsync();

            base.OnExit(e);
        }



        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //view principal
            services.AddTransient<MainWindow>();

            //view viewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegistroViewModel>();
            services.AddTransient<PrincipalViewModel>();
            services.AddTransient<EventosViewModel>();
            services.AddTransient<AddAsignaturaViewModel>();
            services.AddTransient<AddEventoViewModel>();
            services.AddTransient<DetallesAsignaturaViewModel>();
            services.AddTransient<PomodoroViewModel>();
            services.AddTransient<CargarViewModel>();

            //Services 
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginDTO>();
            services.AddSingleton<IEventoNotificacionProvider, EventoNotificacionService>();
            services.AddSingleton<IEmailSenderProvider, EmailSenderService>();
            services.AddSingleton<INotaApiProvider, NotaApiService>();
            services.AddSingleton<IEventoApiProvider, EventoApiService>();
            services.AddSingleton<IAsignaturaApiProvider, AsignaturaApiService>();
            services.AddSingleton<IUserApiProvider, UserApiService>();
            services.AddSingleton<ICalcularMediaProvider, CalcularMediaService>();
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();
            services.AddSingleton(typeof(IFileProvider<>), typeof(FileService<>));
            services.AddSingleton(typeof(IHttpsJsonClientProvider<>), typeof(HttpsJsonClientService<>));

            return services.BuildServiceProvider();
        }
    }

}

