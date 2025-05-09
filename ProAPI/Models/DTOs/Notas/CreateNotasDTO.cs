﻿using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.Notas
{
    public class CreateNotasDTO
    {
        [Required]
        public double NotaValor { get; set; }

        [Required]
        public int IdAsignatura { get; set; }

        [Required]
        public int IdEvento { get; set; }

        [Required]
        public int IdUsuario { get; set; }
    }
}
