using Magic_Villa_Web.Models.DTO.Villa;

namespace Magic_Villa_Web.Models.ViewModel
{
    public class VillaPaginadoViewModel
    {
        public int PageNumber { get; set; }

        public int TotalPaginas { get; set; }

        public string Previo { get; set; } = "disabled";

        public string Siguiente { get; set; } = "";

        public IEnumerable<VillaDTO> VillaList { get; set; }
    }
}
