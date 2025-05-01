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

            double mediaActual = sumaPorcentajes > 0 ? sumaPonderada / sumaPorcentajes : 0;

            if (sumaPorcentajes > 1)
            {
                return $"Media actual: {mediaActual:F2}.\n La suma de porcentajes es {sumaPorcentajes * 100}%. Hay eventos con porcentajes mal introducidos.";
            }
            else if (sumaPorcentajes < 1)
            {
                double porcentajeFaltante = 1 - sumaPorcentajes;
                double notaNecesaria = (5.0 - sumaPonderada) / porcentajeFaltante;

                if (notaNecesaria > 10)
                    return $"Media actual: {mediaActual:F2}.\n Aunque saques un 10 en el resto, no llegarías al 5.";
                else if (notaNecesaria < 0)
                    return $"Media actual: {mediaActual:F2}.\n Ya tienes suficiente media para aprobar, aunque no pongas más notas.";
                else
                    return $"Media actual: {mediaActual:F2}.\n Te falta un {porcentajeFaltante * 100}% de eventos. Necesitas una media de al menos {notaNecesaria:F2} en lo que falta para aprobar.";
            }
            else
            {
                return $"Media final: {mediaActual:F2}";
            }
        }

        public async Task<string> CalcularFaltas(int IdAsignatura)
        {
            var asignatura = await _asignaturaService.GetOneAsignatura(IdAsignatura.ToString());

            if (asignatura == null)
                return "La asignatura no se ha encontrado.";

            if (asignatura.Horas <= 0)
                return "No se han registrado horas en la asignatura.";

            double porcentajeFaltasConsumidas = ((double)asignatura.Faltas / asignatura.Horas) * 100;
            double porcentajeFaltasPermitidas = asignatura.PorcentajeFaltas;
            double porcentajeFaltasRestantes = porcentajeFaltasPermitidas - porcentajeFaltasConsumidas;

            double faltasRestantes = (asignatura.Horas * porcentajeFaltasRestantes) / 100;

            if (porcentajeFaltasRestantes < 0)
            {
                double faltasExcedidas = asignatura.Faltas - ((asignatura.Horas * porcentajeFaltasPermitidas) / 100);
                return $"Has superado el límite de faltas.\n" +
                       $"- Has faltado un {porcentajeFaltasConsumidas:F2}% del total de horas.\n" +
                       $"- Te has pasado por {faltasExcedidas:F2} faltas.";
            }

            return $"Te quedan {faltasRestantes:F2} faltas ({porcentajeFaltasRestantes:F2}%) permitidas para no perder la evaluación.";
        }
    }
}
