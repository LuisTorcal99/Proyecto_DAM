﻿using RestAPI.Controllers;
using RestAPI.Models.DTOs.Notas;

namespace RestAPI.Models.DTOs.Evento
{
    public class EventoDTO : CreateEventoDTO
    {
        public int Id { get; set; }
        public NotasDTO Nota { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
