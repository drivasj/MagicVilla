using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MagicVilla_API.Modelos.DTO.Villa;

namespace MagicVilla_API.Modelos.DTO.NumeroVilla
{
    public class NumeroVillaDTO
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }

        public VillaDTO Villa { get; set; }
    }
}
