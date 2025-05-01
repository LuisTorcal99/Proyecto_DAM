using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;

namespace Proyecto_DAM.ViewModel
{
    public partial class PerfilViewModel : ViewModelBase
    {
        private readonly IActualizarPerfilProvider _actualizarPerfilService;
        private readonly IAsignaturaApiProvider _asignaturaApiService;
        private readonly INotaApiProvider _notaApiService;
        private readonly IEventoApiProvider _eventoApiService;

        [ObservableProperty]
        public string _Name;

        [ObservableProperty]
        public string _UserName;

        [ObservableProperty]
        public string _Email;

        [ObservableProperty]
        private int _TotalAsignaturas;

        [ObservableProperty]
        private int _TotalEventos;

        [ObservableProperty]
        private int _TotalAprobados;

        [ObservableProperty]
        private int _TotalSuspendidos;

        [ObservableProperty]
        private int _TotalPendientes;


        public PerfilViewModel(IActualizarPerfilProvider actualizarPerfilProvider,
                                IAsignaturaApiProvider asignaturaApiProvider,
                                IEventoApiProvider eventoApiProvider,
                                INotaApiProvider notaApiProvider)
        {
            _actualizarPerfilService = actualizarPerfilProvider;
            _asignaturaApiService = asignaturaApiProvider;
            _eventoApiService = eventoApiProvider;
            _notaApiService = notaApiProvider;
        }

        [RelayCommand]
        public async Task Editar()
        {
            var loginDto = App.Current.Services.GetService<LoginDTO>();
            if (loginDto == null)
                return;

            var aspNetUser = await _actualizarPerfilService.ObtenerUserAspNetPorId(loginDto.Id);

            if (aspNetUser == null)
            {
                Console.WriteLine("No se pudieron obtener los datos del usuario.");
                return;
            }

            bool datosModificados = false;

            if ( aspNetUser.Name != Name)
            {
                aspNetUser.Name = Name;
                datosModificados = true;
            }

            if (aspNetUser.Email != Email)
            {
                aspNetUser.Email = Email;
                datosModificados = true;
            }

            if (aspNetUser.UserName != UserName)
            {
                aspNetUser.UserName = UserName;
                datosModificados = true;
            }

            if (!datosModificados)
            {
                MessageBox.Show("No se han realizado cambios.");
                return;
            }

            bool actualizado = await _actualizarPerfilService.ActualizarAspNetUser(aspNetUser);

            if (actualizado)
            {
                MessageBox.Show("Perfil actualizado correctamente.");
            }
            else
            {
                MessageBox.Show("Hubo un problema al actualizar el perfil.");
            }
        }

        public override async Task LoadAsync()
        {
            var loginDto = App.Current.Services.GetService<LoginDTO>();
            if (loginDto == null)
                return;

            var aspNetUser = await _actualizarPerfilService.ObtenerUserAspNetPorId(loginDto.Id);

            if (aspNetUser != null)
            {
                Name = aspNetUser.Name;
                Email = aspNetUser.Email;
                UserName = aspNetUser.UserName;
            }

            try
            {
                var asignaturas = await _asignaturaApiService.GetAsignatura();
                TotalAsignaturas = asignaturas?.ToList().Count ?? 0;

                var eventos = await _eventoApiService.GetEvento();
                TotalEventos = eventos?.ToList().Count ?? 0;
                
                var notas = await _notaApiService.GetNota();
                TotalAprobados = notas?.Count(n => n.NotaValor >= 5) ?? 0;
                TotalSuspendidos = notas?.Count(n => n.NotaValor >= 0 && n.NotaValor < 5) ?? 0;
                TotalPendientes = TotalEventos - (TotalAprobados + TotalSuspendidos);
                if (TotalPendientes < 0) TotalPendientes = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando datos del perfil: {ex.Message}");
            }
        }
    }
}
