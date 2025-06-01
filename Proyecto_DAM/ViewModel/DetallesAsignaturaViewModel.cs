using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Proyecto_DAM.RabbitMQ;
using System.Text.Json;
using System.Globalization;
using System.Net.Http;
using DocumentFormat.OpenXml.Office2010.Excel;
using Proyecto_DAM.View;

namespace Proyecto_DAM.ViewModel
{
    public partial class DetallesAsignaturaViewModel : ViewModelBase
    {
        private readonly IAsignaturaApiProvider _asignaturaService;
        private readonly IEventoApiProvider _eventoService;
        private readonly INotaApiProvider _notaService;
        private readonly ICalcularMediaProvider _calcularMediaService;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private AsignaturaDTO? _Asignatura;

        [ObservableProperty]
        public string _MediaResultado;

        [ObservableProperty]
        public string _FaltasRestantesParaPerderEvaluacion;

        public ObservableCollection<string> TiposEvento { get; set; }
        public ObservableCollection<string> EstadosEvento { get; set; }

        public DetallesAsignaturaViewModel(IAsignaturaApiProvider asignaturaApiProvider,
                                            IEventoApiProvider eventoService,
                                            INotaApiProvider notaService,
                                            ICalcularMediaProvider calcularMediaService,
                                            IRabbitMQProducer rabbitMQProducer,
                                            IServiceProvider serviceProvider)
        {
            _asignaturaService = asignaturaApiProvider;
            _eventoService = eventoService;
            _notaService = notaService;
            _calcularMediaService = calcularMediaService;
            _rabbitMQProducer = rabbitMQProducer;

            TiposEvento = new ObservableCollection<string>() { "Nota", "Tarea", "Examen" };
            EstadosEvento = new ObservableCollection<string>() { "Pendiente", "EnProceso", "Completado" };
            _serviceProvider = serviceProvider;
        }

        public async Task SetIdAsignatura(int id)
        {
            await CargarDetalles(id.ToString());
        }

        [RelayCommand]
        public async Task Guardar(EventoDTO evento)
        {
            if (evento == null || evento.Nota == null) return;

            // Obtener las notas asociadas a los eventos
            var notas = await _notaService.GetNota();

            // Buscar si existe una nota asociada al evento con el idEvento
            var notaExistente = notas.FirstOrDefault(n => n.IdEvento == evento.Id);

            try
            {
                var notaValor = evento.Nota;

                // Si la nota es null, significa que no se ha asignado una nota
                if (evento.Nota == null || evento.Nota == notaExistente.NotaValor)
                {
                    await _eventoService.PatchEvento(evento);
                    MessageBox.Show("Estado o Tipo guardados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    var mensaje = new MensajeRabbit
                    {
                        Tipo = "Evento",
                        Contenido = $"Evento actualizado: {evento.Nombre} (Tipo: {evento.Tipo}, Estado: {evento.Estado})"
                    };
                    await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
                    return;
                }

                // Si la nota no está en el rango de 0 a 10, se ignora
                if (notaValor < 0 || notaValor > 10)
                {
                    return;
                }

                // Verificamos si la nota actual no coincide con la que está en el objeto evento
                if (notaExistente == null)
                {
                    // Si la nota es NULL, se crea una nueva y se hace un POST
                    var newNota = new NotaDTO
                    {
                        NotaValor = (double) evento.Nota,
                        IdAsignatura = evento.IdAsignatura,
                        IdEvento = evento.Id,
                        IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                    };

                    // Realizamos el POST para guardar la nueva nota
                    await _notaService.PostNota(newNota);
                    evento.Nota = newNota.NotaValor;
                    await _eventoService.PatchEvento(evento);
                    MediaResultado = await _calcularMediaService.CalcularMedia(StringUtils.ConvertToNumberNoPanic(Asignatura.Id.ToString()));

                    var mensaje = new MensajeRabbit
                    {
                        Tipo = "Evento",
                        Contenido = $"Evento actualizado: {evento.Nombre} Nota: {evento.Nota})"
                    };
                    await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
                    App.Current.Services.GetService<MainViewModel>().SelectViewModelCommand.Execute(App.Current.Services.GetService<PrincipalViewModel>());
                }
                else
                {
                    // Si la nota existe, la actualizamos
                    var updatedNota = new NotaDTO
                    {
                        Id = notaExistente.Id,
                        NotaValor = (double)evento.Nota,
                        IdAsignatura = evento.IdAsignatura,
                        IdEvento = evento.Id,
                        IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                    };

                    // Realizamos el PATCH para actualizar la nota
                    await _notaService.PatchNota(updatedNota);
                    evento.Nota = updatedNota.NotaValor;
                    await _eventoService.PatchEvento(evento);
                    MediaResultado = await _calcularMediaService.CalcularMedia(StringUtils.ConvertToNumberNoPanic(Asignatura.Id.ToString()));

                    var mensaje = new MensajeRabbit
                    {
                        Tipo = "Evento",
                        Contenido = $"Evento actualizado: {evento.Nombre} Nota: {evento.Nota})"
                    };
                    await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
                    App.Current.Services.GetService<MainViewModel>().SelectViewModelCommand.Execute(App.Current.Services.GetService<PrincipalViewModel>());
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

                    evento.Nota = null;
                    await _eventoService.PatchEvento(evento);

                    if (notaAsociada != null)
                    {
                        await _notaService.DeleteNota(notaAsociada.Id.ToString());
                    }

                    await _eventoService.DeleteEvento(evento.Id.ToString());

                    var mensaje = new MensajeRabbit
                    {
                        Tipo = "Evento",
                        Contenido = $"Evento Borrado: {evento.Nombre} Tipo: {evento.Tipo})"
                    };
                    await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));

                    App.Current.Services.GetService<MainViewModel>().SelectViewModelCommand.Execute(App.Current.Services.GetService<PrincipalViewModel>());

                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w.IsActive)?
                        .Close();

                    MessageBox.Show("Evento eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    var viewModel = _serviceProvider.GetRequiredService<DetallesAsignaturaViewModel>();
                    await viewModel.SetIdAsignatura(Asignatura.Id);

                    var view = new DetallesAsignaturaView { DataContext = viewModel };
                    view.ShowDialog();
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

                    var mensaje = new MensajeRabbit
                    {
                        Tipo = "Evento",
                        Contenido = $"Asignatura eliminada: {Asignatura.Nombre})"
                    };
                    await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));

                    App.Current.Services.GetService<MainViewModel>().SelectViewModelCommand.Execute(App.Current.Services.GetService<PrincipalViewModel>());

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
                FaltasRestantesParaPerderEvaluacion = await _calcularMediaService.CalcularFaltas(StringUtils.ConvertToNumberNoPanic(id));

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
                            evento.Nota = nota.NotaValor;
                        }
                        else
                        {
                            evento.Nota = null;
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

        [RelayCommand]
        public async Task GuardarCambios()
        {
            try
            {
                var faltas = Asignatura.Faltas;
                var porcentajeFaltas = Asignatura.PorcentajeFaltas;

                var asignaturaUpdate = new AsignaturaDTO
                {
                    Id = Asignatura.Id,
                    Nombre = Asignatura.Nombre,
                    Descripcion = Asignatura.Descripcion,
                    Creditos = Asignatura.Creditos,
                    Horas = Asignatura.Horas,
                    IdUsuario = App.Current.Services.GetService<LoginDTO>().Id,
                    Eventos = Asignatura.Eventos,
                    Notas = Asignatura.Notas,
                    
                    Faltas = faltas,
                    PorcentajeFaltas = porcentajeFaltas
                };

                await _asignaturaService.PatchAsignatura(asignaturaUpdate);

                var mensaje = new MensajeRabbit
                {
                    Tipo = "Evento",
                    Contenido = $"Asignatura actualizada: {Asignatura.Nombre}"
                };
                await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
                MessageBox.Show("Asignatura actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                FaltasRestantesParaPerderEvaluacion = await _calcularMediaService.CalcularFaltas(StringUtils.ConvertToNumberNoPanic(Asignatura.Id.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios: " + ex.Message);
            }
        }

        [RelayCommand]
        public async Task AddEvento()
        {
            var addEventoViewModel = App.Current.Services.GetService<AddEventoViewModel>();

            if (addEventoViewModel == null)
            {
                MessageBox.Show("No se pudo cargar el ViewModel.");
                return;
            }

            addEventoViewModel.Asignatura = this.Asignatura;

            // Cierra la ventana actual (activa)
            var currentWindow = Application.Current.Windows.OfType<Window>()
                                 .SingleOrDefault(x => x.IsActive);
            currentWindow?.Close();

            // Abre ventana modal de agregar evento
            var addEventoView = new AddEventoView
            {
                DataContext = addEventoViewModel
            };
            bool? dialogResult = addEventoView.ShowDialog();

            // Después de cerrar AddEventoView, abre la vista de detalles
            var detallesViewModel = App.Current.Services.GetRequiredService<DetallesAsignaturaViewModel>();
            await detallesViewModel.SetIdAsignatura(Asignatura.Id);

            var detallesView = new DetallesAsignaturaView
            {
                DataContext = detallesViewModel
            };
            detallesView.ShowDialog();
        }

        public override Task LoadAsync()
        {
            return base.LoadAsync();
        }
    }
}
