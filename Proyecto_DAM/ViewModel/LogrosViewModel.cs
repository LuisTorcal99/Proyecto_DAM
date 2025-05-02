using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_DAM.Utils;
using System.Collections.ObjectModel;

namespace Proyecto_DAM.ViewModel
{
    public partial class LogrosViewModel : ViewModelBase
    {
        private readonly ILogrosApiProvider _logrosService;
        private readonly IAsignaturaApiProvider _asignaturaApiService;
        private readonly INotaApiProvider _notaApiProvider;
        private readonly IUserApiProvider _usuarioService;
        public ObservableCollection<GamificacionDTO> Logros { get; set; }
        public ObservableCollection<RankingDTO> RankingUsuarios { get; set; }

        [ObservableProperty]
        public int _LogrosRestantes;

        [ObservableProperty]
        public int _PuntosTotales;

        public LogrosViewModel(INotaApiProvider notaApiProvider, IAsignaturaApiProvider asignaturaApiProvider, ILogrosApiProvider logrosService, IUserApiProvider usuarioService)
        {
            Logros = new ObservableCollection<GamificacionDTO>();
            RankingUsuarios = new ObservableCollection<RankingDTO>();

            _notaApiProvider = notaApiProvider;
            _asignaturaApiService = asignaturaApiProvider;
            _logrosService = logrosService;
            _usuarioService = usuarioService;
        }

        public override async Task LoadAsync()
        {
            await CalcularLogros();
            await CalcularRanking();
        }

        public async Task CalcularLogros()
        {
            Logros.Clear();
            int usuarioId = App.Current.Services.GetService<LoginDTO>().Id;
            DateTime ahora = DateTime.Now;

            var asignaturas = await _asignaturaApiService.GetAsignatura();

            double tiempoEstudio = 0;
            foreach (var asignatura in asignaturas)
            {
                var tiempo = await _asignaturaApiService.GetTiempoEstudio(asignatura.Id);
                if (tiempo != null)
                {
                    tiempoEstudio += tiempo.TiempoEstudiado.TotalHours;
                }
            }

            var notas = await _notaApiProvider.GetNota();
            int notasAprobadas = notas.Count(n => n.IdUsuario == usuarioId && n.NotaValor >= 5);

            // Logros por tiempo de estudio
            if (tiempoEstudio >= 1)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 10, Descripcion = "¡Primer paso! Has estudiado al menos 1 hora." });
            if (tiempoEstudio >= 10)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 20, Descripcion = "¡Constante! 10 horas de estudio acumuladas." });
            if (tiempoEstudio >= 50)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 40, Descripcion = "¡Dedicado! 50 horas de estudio alcanzadas." });
            if (tiempoEstudio >= 100)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 60, Descripcion = "¡Impresionante! 100 horas de estudio." });
            if (tiempoEstudio >= 200)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 100, Descripcion = "¡Maestro del estudio! Más de 200 horas acumuladas." });

            // Logros por notas aprobadas
            if (notasAprobadas >= 1)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 10, Descripcion = "¡Primera victoria! Has aprobado tu primera nota." });
            if (notasAprobadas >= 5)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 20, Descripcion = "¡Buen comienzo! 5 notas aprobadas." });
            if (notasAprobadas >= 10)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 40, Descripcion = "¡Vas en serio! 10 notas aprobadas." });
            if (notasAprobadas >= 15)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 60, Descripcion = "¡Excelente! 15 asignaturas aprobadas." });
            if (notasAprobadas >= 20)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 100, Descripcion = "¡Crack académico! Más de 20 aprobadas." });

            int logrosEstudio = Logros.Count(l => l.TipoDeLogro == "Estudio");
            if (logrosEstudio == 5)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Especial", Puntos = 50, Descripcion = "¡Has conseguido todos los logros de estudio!" });

            int logrosAprobados = Logros.Count(l => l.TipoDeLogro == "Aprobados");
            if (logrosAprobados == 5)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Especial", Puntos = 50, Descripcion = "¡Todos los logros de aprobados desbloqueados!" });

            // Calcular el total de puntos
            int puntosAcumulados = Logros.Sum(l => l.Puntos);

            // Otros logros por puntos
            if (puntosAcumulados >= 100)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Puntos", Puntos = 10, Descripcion = "¡Ya llevas 100 puntos!" });
            if (puntosAcumulados >= 300)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Puntos", Puntos = 30, Descripcion = "¡Maestro en camino! Has alcanzado 300 puntos." });
            if (puntosAcumulados >= 500)
                Logros.Add(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Puntos", Puntos = 50, Descripcion = "¡Leyenda! Más de 500 puntos conseguidos." });

            // Obtener los logros existentes desde la base de datos
            var logrosExistentes = await _logrosService.GetLogros();

            int logrosObtenidos = Logros.Count();

            // Comparar y agregar logros nuevos
            var logrosNuevos = new List<GamificacionDTO>();
            foreach (var logro in Logros)
            {
                if (!logrosExistentes.Any(l => l.TipoDeLogro == logro.TipoDeLogro && l.Descripcion == logro.Descripcion))
                {
                    logrosNuevos.Add(logro);
                }
            }

            // Si hay logros nuevos, hacer un POST para agregarlos
            if (logrosNuevos.Count > 0)
            {
                // Iteramos sobre cada logro nuevo
                foreach (var logro in logrosNuevos)
                {
                    // Convertimos el logro a un GamificacionDTO
                    var gamificacionDTO = new GamificacionDTO
                    {
                        UsuarioId = logro.UsuarioId,
                        Fecha = logro.Fecha,
                        TipoDeLogro = logro.TipoDeLogro,
                        Puntos = logro.Puntos,
                        Descripcion = logro.Descripcion
                    };

                    // Hacemos el POST del logro
                    await _logrosService.PostLogro(gamificacionDTO);
                }
            }
            
            PuntosTotales = puntosAcumulados;
            LogrosRestantes = Constantes.LOGROS_TOTALES - logrosObtenidos;
        }

        public async Task CalcularRanking()
        {
            RankingUsuarios.Clear();
            int posicion = 1;

            // Obtener todos los logros desde la base de datos
            var logrosExistentes = await _logrosService.GetLogrosRanking();

            var logrosPorUsuario = logrosExistentes
                .GroupBy(l => l.UsuarioId)
                .Select(group => new
                {
                    UsuarioId = group.Key,
                    TotalLogros = group.Count()
                })
                .OrderByDescending(user => user.TotalLogros)
                .ToList();

            foreach (var usuario in logrosPorUsuario)
            {
                if(posicion == 10)
                    break;

                var usuarioData = await _usuarioService.GetOneUser(usuario.UsuarioId.ToString());
                if (usuarioData != null)
                {
                    RankingUsuarios.Add(new RankingDTO
                    {
                        Posicion = posicion++,
                        NombreUsuario = usuarioData.Nombre,
                        TotalLogros = usuario.TotalLogros
                    });
                }
            }
        }
    }

    public class RankingDTO
    {
        public int Posicion { get; set; }
        public string NombreUsuario { get; set; }
        public int TotalLogros { get; set; }
    }
}
