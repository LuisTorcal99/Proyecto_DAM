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

            var logros = await _logrosService.GetLogros();

            foreach (var logroExistente in logros)
            {
                Logros.Add(logroExistente);
            }

            var logrosNuevos = new List<GamificacionDTO>();

            void AgregarSiNoExiste(GamificacionDTO logro)
            {
                if (!logros.Any(l => l.TipoDeLogro == logro.TipoDeLogro && l.Descripcion == logro.Descripcion))
                {
                    logrosNuevos.Add(logro);
                }
            }

            var now = DateTime.Now;

            // Logros por tiempo de estudio
            if (tiempoEstudio >= 1)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 10, Descripcion = "¡Primer paso! Has estudiado al menos 1 hora." });
            if (tiempoEstudio >= 10)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 20, Descripcion = "¡Constante! 10 horas de estudio acumuladas." });
            if (tiempoEstudio >= 50)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 40, Descripcion = "¡Dedicado! 50 horas de estudio alcanzadas." });
            if (tiempoEstudio >= 100)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 60, Descripcion = "¡Impresionante! 100 horas de estudio." });
            if (tiempoEstudio >= 200)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Estudio", Puntos = 100, Descripcion = "¡Maestro del estudio! Más de 200 horas acumuladas." });

            // Logros por notas aprobadas
            if (notasAprobadas >= 1)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 10, Descripcion = "¡Primera victoria! Has aprobado tu primera nota." });
            if (notasAprobadas >= 5)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 20, Descripcion = "¡Buen comienzo! 5 notas aprobadas." });
            if (notasAprobadas >= 10)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 40, Descripcion = "¡Vas en serio! 10 notas aprobadas." });
            if (notasAprobadas >= 15)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 60, Descripcion = "¡Excelente! 15 asignaturas aprobadas." });
            if (notasAprobadas >= 20)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Aprobados", Puntos = 100, Descripcion = "¡Crack académico! Más de 20 aprobadas." });

            // Logros especiales por completar categoría
            int logrosEstudio = logros.Count(l => l.TipoDeLogro == "Estudio") + logrosNuevos.Count(l => l.TipoDeLogro == "Estudio");
            if (logrosEstudio == 5)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Especial", Puntos = 50, Descripcion = "¡Has conseguido todos los logros de estudio!" });

            int logrosAprobados = logros.Count(l => l.TipoDeLogro == "Aprobados") + logrosNuevos.Count(l => l.TipoDeLogro == "Aprobados");
            if (logrosAprobados == 5)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Especial", Puntos = 50, Descripcion = "¡Todos los logros de aprobados desbloqueados!" });

            // Calcular puntos totales acumulados
            int puntosAcumulados = logros.Sum(l => l.Puntos) + logrosNuevos.Sum(l => l.Puntos);

            // Logros por puntos acumulados
            if (puntosAcumulados >= 100)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Puntos", Puntos = 10, Descripcion = "¡Ya llevas 100 puntos!" });
            if (puntosAcumulados >= 300)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Puntos", Puntos = 30, Descripcion = "¡Maestro en camino! Has alcanzado 300 puntos." });
            if (puntosAcumulados >= 500)
                AgregarSiNoExiste(new GamificacionDTO { UsuarioId = usuarioId, Fecha = ahora, TipoDeLogro = "Puntos", Puntos = 50, Descripcion = "¡Leyenda! Más de 500 puntos conseguidos." });

            // Logro de haber conseguido todos los logros
            if (logrosEstudio == 5 && logrosAprobados == 5)
            {
                AgregarSiNoExiste(new GamificacionDTO
                {
                    UsuarioId = usuarioId,
                    Fecha = ahora,
                    TipoDeLogro = "Especial",
                    Puntos = 350,
                    Descripcion = "¡Conseguiste todos los logros!"
                });
            }

            // Finalmente, agregar los logros nuevos a la colección y a la API
            foreach (var nuevo in logrosNuevos)
            {
                Logros.Add(nuevo);
                await _logrosService.PostLogro(nuevo);
            }

            puntosAcumulados = logros.Sum(l => l.Puntos) + logrosNuevos.Sum(l => l.Puntos);
            PuntosTotales = puntosAcumulados;
            LogrosRestantes = Constantes.LOGROS_TOTALES - (logros.Count() + logrosNuevos.Count);
        }

        public async Task CalcularRanking()
        {
            RankingUsuarios.Clear();
            int posicion = 1;

            var logrosExistentes = await _logrosService.GetLogrosRanking();

            var logrosPorUsuario = logrosExistentes
                .GroupBy(l => l.UsuarioId)
                .Select(group => new
                {
                    UsuarioId = group.Key,
                    TotalLogros = group.Count(),
                    UltimoLogro = group.Min(l => l.Fecha)
                })
                .OrderByDescending(user => user.TotalLogros)
                .ThenBy(user => user.UltimoLogro)
                .ToList();

            foreach (var usuario in logrosPorUsuario)
            {
                if (posicion == 10)
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

        public class RankingDTO
        {
            public int Posicion { get; set; }
            public string NombreUsuario { get; set; }
            public int TotalLogros { get; set; }
        }
    }
}
