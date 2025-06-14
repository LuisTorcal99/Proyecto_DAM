﻿using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.Entity
{
    public class NotaEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double NotaValor { get; set; }
        public int IdAsignatura { get; set; }
        public int IdEvento { get; set; }
        public int IdUsuario { get; set; }
        public User Usuario { get; set; }
    }
}
