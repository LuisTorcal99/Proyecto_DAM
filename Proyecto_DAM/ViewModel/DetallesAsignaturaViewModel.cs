using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_DAM.ViewModel
{
    public partial class DetallesAsignaturaViewModel : ViewModelBase
    {
        private readonly IAsignaturaApiProvider _asignaturaService;
        private readonly IEventoApiProvider _eventoService;
        private readonly INotaApiProvider _notaService;
        private readonly ICalcularMediaProvider _calcularMediaService;

        [ObservableProperty]
        private AsignaturaDTO? _Asignatura;

        [ObservableProperty]
        public string _MediaResultado;

        public ObservableCollection<string> TiposEvento { get; set; }
        public ObservableCollection<string> EstadosEvento { get; set; }

        public DetallesAsignaturaViewModel(IAsignaturaApiProvider asignaturaApiProvider, 
                                            IEventoApiProvider eventoService, 
                                            INotaApiProvider notaService,
                                            ICalcularMediaProvider calcularMediaService)
        {
            _asignaturaService = asignaturaApiProvider;
            _eventoService = eventoService;
            _notaService = notaService;
            _calcularMediaService = calcularMediaService;

            TiposEvento = new ObservableCollection<string>() { "Tarea", "Examen" };
            EstadosEvento = new ObservableCollection<string>() { "Pendiente", "EnProceso", "Estudiado" };
        }
        public async Task SetIdAsignatura(int id)
        {
            await CargarDetalles(id.ToString());
        }

        [RelayCommand]
        public async Task Guardar(EventoDTO evento)
        {
            if (evento == null || evento.Nota == null) return;

            try
            {
                var notaValor = evento.Nota.NotaValor;

                // Si la nota no está en el rango de 0 a 10, se ignora
                if (notaValor < 0 || notaValor > 10)
                {
                    await _eventoService.PatchEvento(evento);
                    MessageBox.Show("Estado o Tipo guardados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Obtener las notas asociadas a los eventos
                var notas = await _notaService.GetNota();

                // Buscar si existe una nota asociada al evento con el idEvento
                var notaExistente = notas.FirstOrDefault(n => n.IdEvento == evento.Id);

                // Verificamos si la nota actual no coincide con la que está en el objeto evento
                if (notaExistente == null)
                {
                    // Si la nota es NULL, se crea una nueva y se hace un POST
                    var newNota = new NotaDTO
                    {
                        NotaValor = evento.Nota.NotaValor,
                        IdAsignatura = evento.IdAsignatura, 
                        IdEvento = evento.Id,
                        IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                    };

                    // Realizamos el POST para guardar la nueva nota
                    await _notaService.PostNota(newNota);
                    evento.Nota = newNota;
                    await _eventoService.PatchEvento(evento);
                }
                else
                {
                    // Si la nota existe, la actualizamos
                    var updatedNota = new NotaDTO
                    {
                        Id = notaExistente.Id,
                        NotaValor = evento.Nota.NotaValor,
                        IdAsignatura = evento.IdAsignatura,
                        IdEvento = evento.Id,
                        IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                    };

                    // Realizamos el PATCH para actualizar la nota
                    await _notaService.PatchNota(updatedNota);
                    evento.Nota = updatedNota;
                    await _eventoService.PatchEvento(evento);
                }

                MessageBox.Show("Nota, Estado o Tipo guardados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la nota: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public async Task Borrar(EventoDTO evento)
        {
            if (evento == null) return;

            var result = MessageBox.Show($"¿Estás seguro de que quieres borrar el evento \"{evento.Nombre}\"?",
                                         "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var notas = await _notaService.GetNota();

                    var notaAsociada = notas.FirstOrDefault(n => n.IdEvento == evento.Id);

                    if (notaAsociada != null)
                    {
                        await _notaService.DeleteNota(notaAsociada.Id.ToString());

                        evento.Nota = null;

                        await _eventoService.PatchEvento(evento);
                    }

                    await _eventoService.DeleteEvento(evento.Id.ToString());
                    Asignatura?.Eventos?.Remove(evento);

                    MessageBox.Show("Evento eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el evento: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        public async Task EliminarAsignatura()
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
                MediaResultado = await _calcularMediaService.CalcularMedia(StringUtils.ConvertToNumberNoPanic(id));

                if (Asignatura != null)
                {
                    var eventos = await _eventoService.GetEvento();
                    var notas = await _notaService.GetNota();

                    var idUsuario = App.Current.Services.GetService<LoginDTO>().Id;

                    var eventosAsignatura = eventos
                        .Where(e => e.IdAsignatura == Asignatura.Id)
                        .ToList();

                    foreach (var evento in eventosAsignatura)
                    {
                        var nota = notas.FirstOrDefault(n => n.IdEvento == evento.Id && n.IdUsuario == idUsuario);

                        if (nota != null)
                        {
                            evento.Nota = nota;
                        }
                        else
                        {
                            evento.Nota = new NotaDTO
                            {
                                NotaValor = -1,
                                IdEvento = evento.Id,
                                IdAsignatura = Asignatura.Id,
                                IdUsuario = idUsuario
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
