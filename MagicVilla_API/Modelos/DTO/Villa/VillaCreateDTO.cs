using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.DTO.Villa
{
    public class VillaCreateDTO
    {
      //  public int Id { get; set; }

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
