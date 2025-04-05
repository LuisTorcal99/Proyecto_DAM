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
        private ObservableCollection<AsignaturaItemModel> _AsignaturaItems;

        public PrincipalViewModel(IAsignaturaApiProvider asignaturaService, IHttpsJsonClientProvider<AsignaturaDTO> httpService)
        {
            _asignaturaService = asignaturaService;
            _httpService = httpService;
            AsignaturaItems = new ObservableCollection<AsignaturaItemModel>();
        }

        public override async Task LoadAsync()
        {
            AsignaturaItems.Clear();

            var asignaturas = await _asignaturaService.GetAsignatura();

            if (asignaturas != null)
            {
                foreach (var asignatura in asignaturas)
                {
                    AsignaturaItems.Add(AsignaturaItemModel.CreateModelFromDTO(asignatura));
                }
            }
            else
            {
                MessageBox.Show(Constantes.MSG_ERROR);
            }
        }
    }
}