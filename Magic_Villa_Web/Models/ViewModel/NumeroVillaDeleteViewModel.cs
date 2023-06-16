using Magic_Villa_Web.Models.DTO.NumeroVilla;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magic_Villa_Web.Models.ViewModel
{
    public class NumeroVillaDeleteViewModel
    {
        public NumeroVillaDeleteViewModel()
        {
            NumeroVilla = new NumeroVillaDTO();
        }

        public NumeroVillaDTO NumeroVilla { get; set; }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
