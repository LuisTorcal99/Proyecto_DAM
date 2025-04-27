using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_DAM.DTO;
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
        public async Task<string> CalcularFaltas(int IdAsignatura)
        {
            // Obtener la asignatura con el id proporcionado
            var asignatura = await _asignaturaService.GetOneAsignatura(IdAsignatura.ToString());

            // Comprobar si se ha encontrado la asignatura
            if (asignatura == null)
                return "La asignatura no se ha encontrado.";

            // Comprobar si las horas totales están registradas
            if (asignatura.Horas <= 0)
                return "No se han registrado horas en la asignatura.";

            // Calcular el porcentaje de faltas consumidas
            double porcentajeFaltasConsumidas = ((double)asignatura.Faltas / asignatura.Horas) * 100;

            // Obtener el porcentaje de faltas permitido directamente desde la asignatura
            double porcentajeFaltasPermitidas = asignatura.PorcentajeFaltas;

            // Calcular el porcentaje de faltas restantes
            double porcentajeFaltasRestantes = porcentajeFaltasPermitidas - porcentajeFaltasConsumidas;

            // Calcular el número de faltas restantes
            double faltasRestantes = (asignatura.Horas * porcentajeFaltasRestantes) / 100;

            // Verificar si ya se han superado las faltas permitidas
            if (porcentajeFaltasRestantes < 0)
                return "Ya has superado el límite de faltas permitidas para esta asignatura.";

            // Devolver el porcentaje y el número de faltas restantes
            return $"Te quedan {faltasRestantes:F2} faltas ({porcentajeFaltasRestantes:F2}%) permitidas para no perder la evaluación.";
        }
    }
}
