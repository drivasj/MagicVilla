using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Magic_Villa_Web.Models.DTO.Villa;

namespace Magic_Villa_Web.Models.DTO.NumeroVilla
{
    public class NumeroVillaDTO
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }

        /// Navegacion hacia el modeo de villa DTO
        public VillaDTO Villa { get; set; } 
    }
}
