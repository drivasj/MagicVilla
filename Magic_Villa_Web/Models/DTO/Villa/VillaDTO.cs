﻿using System.ComponentModel.DataAnnotations;

namespace Magic_Villa_Web.Models.DTO.Villa
{
    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string ImagenURL { get; set; }

        public string Amenidad { get; set; }

    }
}
