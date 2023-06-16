﻿using System.ComponentModel.DataAnnotations;

namespace Magic_Villa_Web.Models.DTO.Villa
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }
        [Required]
        public int MetrosCuadrados { get; set; }
        [Required]
        public string ImagenURL { get; set; }

        public string Amenidad { get; set; }

    }
}
