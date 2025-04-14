using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Models;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.ViewModel
{
    public partial class PrincipalViewModel : ViewModelBase
    {
        private readonly IAsignaturaApiProvider _asignaturaService;
        private readonly IHttpsJsonClientProvider<AsignaturaDTO> _httpService;

        [ObservableProperty]
        private ObservableCollection<AsignaturaItemModel> _AsignaturaItem;

        public PrincipalViewModel(IAsignaturaApiProvider asignaturaService, IHttpsJsonClientProvider<AsignaturaDTO> httpService)
        {
            _asignaturaService = asignaturaService;
            _httpService = httpService;
            AsignaturaItem = new ObservableCollection<AsignaturaItemModel>();
        }

        public override async Task LoadAsync()
        {
            AsignaturaItem.Clear();

            try
            {
                var asignaturas = await _asignaturaService.GetAsignatura();

                if (asignaturas?.Any() == true)
                {
                    var asignaturasFiltradas = asignaturas
                        .Where(a => a.IdUsuario.Equals(App.Current.Services.GetService<LoginDTO>().Id))
                        .Select(a => AsignaturaItemModel.CreateModelFromDTO(a))
                        .ToList();

                    foreach (var asignatura in asignaturasFiltradas)
                    {
                        AsignaturaItem.Add(asignatura);
                    }
                }
                else
                {
                    MessageBox.Show(Constantes.MSG_ERROR);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaturas: {ex.Message}");
            }
        }
    }
}