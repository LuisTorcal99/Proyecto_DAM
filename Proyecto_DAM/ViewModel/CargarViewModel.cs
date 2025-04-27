using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Proyecto_DAM.Cursos;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.RabbitMQ;
using static Proyecto_DAM.Models.ExportJsonModel;

namespace Proyecto_DAM.ViewModel
{
    public partial class CargarViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // Propiedades de las listas
        public ObservableCollection<string> Tipos { get; set; }
        public ObservableCollection<string> Modulos { get; set; }
        public ObservableCollection<string> Cursos { get; set; }
        public ObservableCollection<AsignaturaDTO> Asignaturas { get; set; }

        // Propiedades seleccionadas
        private string tipoSeleccionado;
        private string moduloSeleccionado;
        private string cursoSeleccionado;
        private AsignaturaDTO asignaturaSeleccionada;

        public string TipoSeleccionado
        {
            get => tipoSeleccionado;
            set
            {
                if (tipoSeleccionado != value)
                {
                    tipoSeleccionado = value;
                    OnPropertyChanged();
                    CargarModulos();
                }
            }
        }

        public string ModuloSeleccionado
        {
            get => moduloSeleccionado;
            set
            {
                if (moduloSeleccionado != value)
                {
                    moduloSeleccionado = value;
                    OnPropertyChanged();
                    CargarCursos(); 
                }
            }
        }

        public string CursoSeleccionado
        {
            get => cursoSeleccionado;
            set
            {
                if (cursoSeleccionado != value)
                {
                    cursoSeleccionado = value;
                    OnPropertyChanged();
                    CargarAsignaturas();  
                }
            }
        }

        public AsignaturaDTO AsignaturaSeleccionada
        {
            get => asignaturaSeleccionada;
            set
            {
                if (asignaturaSeleccionada != value)
                {
                    asignaturaSeleccionada = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isModuloEnabled;
        private bool isCursoEnabled;
        private bool isAsignaturaEnabled;

        public bool IsModuloEnabled
        {
            get => isModuloEnabled;
            set
            {
                if (isModuloEnabled != value)
                {
                    isModuloEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCursoEnabled
        {
            get => isCursoEnabled;
            set
            {
                if (isCursoEnabled != value)
                {
                    isCursoEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAsignaturaEnabled
        {
            get => isAsignaturaEnabled;
            set
            {
                if (isAsignaturaEnabled != value)
                {
                    isAsignaturaEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly IAsignaturaApiProvider _asignaturaApiService;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        public CargarViewModel(IAsignaturaApiProvider asignaturaApiService, IRabbitMQProducer rabbitMQProducer)
        {
            Tipos = new ObservableCollection<string> { "Grado", "Carrera" };
            Modulos = new ObservableCollection<string>();
            Cursos = new ObservableCollection<string>();
            Asignaturas = new ObservableCollection<AsignaturaDTO>();

            IsModuloEnabled = false;
            IsCursoEnabled = false;
            IsAsignaturaEnabled = false;
            _asignaturaApiService = asignaturaApiService;
            _rabbitMQProducer = rabbitMQProducer;
        }

        public void CargarModulos()
        {
            switch (TipoSeleccionado)
            {
                case "Grado":
                    Modulos = new ObservableCollection<string> { "DAM", "DAW", "ASIR", "Farmacia y Parafarmacia"};
                    break;

                case "Carrera":
                    Modulos = new ObservableCollection<string> { "Ingenieria informatica", "Enfermeria" };
                    break;

                default:
                    break;
            }

            IsModuloEnabled = Modulos.Count > 0;
            OnPropertyChanged(nameof(Modulos));
        }

        public void CargarCursos()
        {
            Cursos.Clear();

            switch (ModuloSeleccionado)
            {
                case "DAM":
                case "DAW":
                case "ASIR":
                case "Farmacia y Parafarmacia":
                    Cursos.Add("1º");
                    Cursos.Add("2º");
                    break;

                case "Ingenieria informatica":
                case "Enfermeria":
                    Cursos.Add("1º");
                    Cursos.Add("2º");
                    Cursos.Add("3º");
                    Cursos.Add("4º");
                    break;

                default:
                    break;
            }

            IsCursoEnabled = Cursos.Count > 0; 
            OnPropertyChanged(nameof(Cursos));
        }

        public void CargarAsignaturas()
        {
            Asignaturas.Clear();

            // Añadir la opción "Todas"
            Asignaturas.Add(new AsignaturaDTO { Nombre = "Todas" });

            switch (TipoSeleccionado)
            {
                case "Grado":
                    switch (ModuloSeleccionado)
                    {
                        case "DAM":
                            switch (CursoSeleccionado)
                            {
                                case "1º":
                                    var asignaturasDAM1 = CursoDAM.ObtenerAsignaturasDAMDAW1();
                                    foreach (var asignatura in asignaturasDAM1)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "2º":
                                    var asignaturasDAM2 = CursoDAM.ObtenerAsignaturasDAM2();
                                    foreach (var asignatura in asignaturasDAM2)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                default:
                                    MessageBox.Show("Curso no válido.");
                                    break;
                            }
                            break;

                        case "DAW":
                            switch (CursoSeleccionado)
                            {
                                case "1º":
                                    var asignaturasDAW1 = CursoDAM.ObtenerAsignaturasDAMDAW1();
                                    foreach (var asignatura in asignaturasDAW1)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "2º":
                                    var asignaturasDAW2 = CursoDAW.ObtenerAsignaturasDAW2();
                                    foreach (var asignatura in asignaturasDAW2)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                default:
                                    MessageBox.Show("Curso no válido.");
                                    break;
                            }
                            break;
                        case "ASIR":
                            switch (CursoSeleccionado)
                            {
                                case "1º":
                                    var asignaturasASIR1 = CursoASIR.ObtenerAsignaturasASIR1();
                                    foreach (var asignatura in asignaturasASIR1)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "2º":
                                    var asignaturasASIR2 = CursoASIR.ObtenerAsignaturasASIR2();
                                    foreach (var asignatura in asignaturasASIR2)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                default:
                                    MessageBox.Show("Curso no válido.");
                                    break;
                            }
                            break;

                        case "Farmacia y Parafarmacia":
                            switch (CursoSeleccionado)
                            {
                                case "1º":
                                    var asignaturasFarmacia1 = CursoFarmaciaParafarmacia.ObtenerAsignaturasFarmacia1();
                                    foreach (var asignatura in asignaturasFarmacia1)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "2º":
                                    var asignaturasFarmacia2 = CursoFarmaciaParafarmacia.ObtenerAsignaturasFarmacia2();
                                    foreach (var asignatura in asignaturasFarmacia2)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                default:
                                    MessageBox.Show("Curso no válido.");
                                    break;
                            }
                            break;

                        default:
                            MessageBox.Show("Módulo no válido.");
                            break;
                    }
                    break;

                case "Carrera":
                    switch (ModuloSeleccionado)
                    {
                        case "Ingenieria informatica":
                            switch (CursoSeleccionado)
                            {
                                case "1º":
                                    var asignaturasIngenieria1 = CursoIngenieriaInformatica.ObtenerAsignaturasIngenieriaInformática1();
                                    foreach (var asignatura in asignaturasIngenieria1)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "2º":
                                    var asignaturasIngenieria2 = CursoIngenieriaInformatica.ObtenerAsignaturasIngenieriaInformática2();
                                    foreach (var asignatura in asignaturasIngenieria2)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "3º":
                                    var asignaturasIngenieria3 = CursoIngenieriaInformatica.ObtenerAsignaturasIngenieriaInformática3();
                                    foreach (var asignatura in asignaturasIngenieria3)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "4º":
                                    var asignaturasIngenieria4 = CursoIngenieriaInformatica.ObtenerAsignaturasIngenieriaInformática4();
                                    foreach (var asignatura in asignaturasIngenieria4)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                default:
                                    MessageBox.Show("Curso no válido.");
                                    break;
                            }
                            break;

                        case "Enfermeria":
                            switch (CursoSeleccionado)
                            {
                                case "1º":
                                    var asignaturasEnfermeria1 = CursoEnfermeria.ObtenerAsignaturasEnfermeria1();
                                    foreach (var asignatura in asignaturasEnfermeria1)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "2º":
                                    var asignaturasEnfermeria2 = CursoEnfermeria.ObtenerAsignaturasEnfermeria2();
                                    foreach (var asignatura in asignaturasEnfermeria2)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "3º":
                                    var asignaturasEnfermeria3 = CursoEnfermeria.ObtenerAsignaturasEnfermeria3();
                                    foreach (var asignatura in asignaturasEnfermeria3)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                case "4º":
                                    var asignaturasEnfermeria4 = CursoEnfermeria.ObtenerAsignaturasEnfermeria4();
                                    foreach (var asignatura in asignaturasEnfermeria4)
                                    {
                                        Asignaturas.Add(asignatura);
                                    }
                                    break;

                                default:
                                    MessageBox.Show("Curso no válido.");
                                    break;
                            }
                            break;

                        default:
                            MessageBox.Show("Módulo no válido.");
                            break;
                    }
                    break;

                default:
                    MessageBox.Show("Tipo no válido.");
                    break;
            }

            IsAsignaturaEnabled = true;
            OnPropertyChanged(nameof(Asignaturas));
            OnPropertyChanged(nameof(IsAsignaturaEnabled));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [RelayCommand]
        public async Task Anadir()
        {
            if (string.IsNullOrEmpty(TipoSeleccionado) || string.IsNullOrEmpty(ModuloSeleccionado) || string.IsNullOrEmpty(CursoSeleccionado) || AsignaturaSeleccionada == null)
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            if(AsignaturaSeleccionada.Nombre == "Todas")
            {
                foreach (var asignatura in Asignaturas)
                {
                    if (asignatura.Nombre != "Todas")
                    {
                        var asignaturaConIDUsuario = new AsignaturaDTO
                        {
                            Nombre = asignatura.Nombre,
                            Descripcion = asignatura.Descripcion,
                            Creditos = asignatura.Creditos,
                            IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                        };
                        await _asignaturaApiService.PostAsignatura(asignaturaConIDUsuario);
                        var mensaje = new MensajeRabbit
                        {
                            Tipo = "Evento",
                            Contenido = $"Asignatura creada: {asignatura.Nombre} (Creditos: {asignatura.Creditos})"
                        };
                        await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
                        App.Current.Services.GetService<MainViewModel>().SelectViewModelCommand.Execute(App.Current.Services.GetService<PrincipalViewModel>());
                    }
                }
                MessageBox.Show($"Curso añadido correctamente.");
            }
            else
            {
                var asignaturaConIDUsuario = new AsignaturaDTO
                {
                    Nombre = AsignaturaSeleccionada.Nombre,
                    Descripcion = AsignaturaSeleccionada.Descripcion,
                    Creditos = AsignaturaSeleccionada.Creditos,
                    IdUsuario = App.Current.Services.GetService<LoginDTO>().Id
                };
                await _asignaturaApiService.PostAsignatura(asignaturaConIDUsuario);
                MessageBox.Show($"Asignatura {AsignaturaSeleccionada.Nombre} añadida correctamente.");
                var mensaje = new MensajeRabbit
                {
                    Tipo = "Evento",
                    Contenido = $"Asignatura creada: {AsignaturaSeleccionada.Nombre} (Creditos: {AsignaturaSeleccionada.Creditos})"
                };
                await _rabbitMQProducer.EnviarMensaje(JsonSerializer.Serialize(mensaje));
                App.Current.Services.GetService<MainViewModel>().SelectViewModelCommand.Execute(App.Current.Services.GetService<PrincipalViewModel>());
            }
        }
    }
}
