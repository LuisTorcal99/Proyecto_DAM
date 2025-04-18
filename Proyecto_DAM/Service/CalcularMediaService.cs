using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto_DAM.Interfaces;

namespace Proyecto_DAM.Service
{
    public class CalcularMediaService : ICalcularMediaProvider
    {
        private readonly IAsignaturaApiProvider _asignaturaService;
        private readonly IEventoApiProvider _eventoService;
        private readonly INotaApiProvider _notaService;

        public CalcularMediaService(IEventoApiProvider eventoService, INotaApiProvider notaService, IAsignaturaApiProvider asignaturaApiService)
        {
            _eventoService = eventoService;
            _notaService = notaService;
            _asignaturaService = asignaturaApiService;
        }

        public async Task<string> CalcularMedia(int idAsignatura)
        {
            var eventos = await _eventoService.GetEvento();
            var notas = await _notaService.GetNota();

            var eventosAsignatura = eventos.Where(e => e.IdAsignatura == idAsignatura).ToList();
            var notasAsignatura = notas
                .Where(n => eventosAsignatura.Any(e => e.Id == n.IdEvento))
                .ToList();

            if (notasAsignatura.Count == 0)
                return "No hay notas registradas para esta asignatura.";

            double sumaPonderada = 0;
            double sumaPorcentajes = 0;

            foreach (var nota in notasAsignatura)
            {
                if (nota.NotaValor == -1)
                    continue;

                var evento = eventosAsignatura.FirstOrDefault(e => e.Id == nota.IdEvento);
                if (evento != null)
                {
                    double porcentaje = evento.Porcentaje / 100.0;
                    sumaPonderada += nota.NotaValor * porcentaje;
                    sumaPorcentajes += porcentaje;
                }
            }

            // ✅ Casos especiales según porcentaje total
            if (sumaPorcentajes > 1)
            {
                return $"⚠️ La suma de porcentajes es {sumaPorcentajes * 100}%. Hay eventos con porcentajes mal introducidos.";
            }
            else if (sumaPorcentajes < 1)
            {
                double porcentajeFaltante = 1 - sumaPorcentajes;

                // Nota necesaria para alcanzar un 5 en total
                double notaNecesaria = (5.0 - sumaPonderada) / porcentajeFaltante;

                if (notaNecesaria > 10)
                    return $"La media actual es insuficiente. Aunque saques un 10 en el resto, no llegarías al 5.";
                else if (notaNecesaria < 0)
                    return $"Ya tienes suficiente media para aprobar, aunque no pongas más notas.";
                else
                    return $"Te falta un {porcentajeFaltante * 100}% de eventos. Necesitas una media de al menos {notaNecesaria:F2} en lo que falta para aprobar.";
            }
            else
            {
                // Todo correcto, devolver la media final
                double mediaFinal = sumaPonderada / sumaPorcentajes;
                return $"✅ Media final: {mediaFinal:F2}";
            }
        }

    }
}
