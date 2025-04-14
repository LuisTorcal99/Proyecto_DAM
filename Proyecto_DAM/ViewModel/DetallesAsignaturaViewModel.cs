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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_DAM.ViewModel
{
    public partial class DetallesAsignaturaViewModel : ViewModelBase
    {
        private readonly IAsignaturaApiProvider _asignaturaService;
        private readonly IEventoApiProvider _eventoService;
        private readonly INotaApiProvider _notaService;

        [ObservableProperty]
        private AsignaturaDTO? _Asignatura;

        public DetallesAsignaturaViewModel(IAsignaturaApiProvider asignaturaApiProvider, 
                                            IEventoApiProvider eventoService, 
                                            INotaApiProvider notaService)
        {
            _asignaturaService = asignaturaApiProvider;
            _eventoService = eventoService;
            _notaService = notaService;
        }
        public async Task SetIdAsignatura(int id)
        {
            await CargarDetalles(id.ToString());
        }

        [RelayCommand]
        public async Task Guardar()
        {
            if (Asignatura == null || Asignatura.Eventos == null) return;

            try
            {
                foreach (var evento in Asignatura.Eventos)
                {
                    if (evento.Nota != null)
                    {
                        var nota = new NotaDTO
                        {
                            NotaValor = evento.Nota.NotaValor,
                            IdAsignatura = Asignatura.Id,
                            IdEvento = evento.Id,
                            IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                        };

                        // Llamamos a la API para guardar la nota
                        await _notaService.PostNota(nota);
                        
                        evento.Nota = nota; 
                        await _eventoService.PatchEvento(evento);
                    }
                }

                MessageBox.Show("Notas guardadas correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar las notas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public async Task Borrar()
        {
            if (Asignatura == null) return;

            var result = MessageBox.Show($"¿Estás seguro de que quieres borrar la asignatura \"{Asignatura.Nombre}\"?",
                                         "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _asignaturaService.DeleteAsignatura(Asignatura.Id.ToString());
                    MessageBox.Show("Asignatura eliminada correctamente.", "Eliminada", MessageBoxButton.OK, MessageBoxImage.Information);

                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w.IsActive)?
                        .Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async Task CargarDetalles(string id)
        {
            try
            {
                Asignatura = await _asignaturaService.GetOneAsignatura(id);

                if (Asignatura != null)
                {
                    var eventos = await _eventoService.GetEvento();
                    var eventosAsignatura = eventos.Where(e => e.IdAsignatura == Asignatura.Id).ToList();

                    // Asegurar que cada evento tenga su nota asociada
                    foreach (var evento in eventosAsignatura)
                    {
                        // Si el evento no tiene una nota, se crea una nueva con valor 0
                        if (evento.Nota == null)
                        {
                            evento.Nota = new NotaDTO
                            {
                                NotaValor = 0,
                                IdEvento = evento.Id,
                                IdAsignatura = Asignatura.Id,
                                IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                            };
                        }
                    }

                    Asignatura.Eventos = eventosAsignatura;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los detalles: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override Task LoadAsync()
        {
            return base.LoadAsync();
        }
    }
}
