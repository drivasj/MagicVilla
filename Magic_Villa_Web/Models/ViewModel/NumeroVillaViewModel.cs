using Magic_Villa_Web.Models.DTO.NumeroVilla;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magic_Villa_Web.Models.ViewModel
{
    public class NumeroVillaViewModel
    {
        public NumeroVillaViewModel()
        {
            NumeroVilla = new NumeroVillaCreateDTO();
        }

        public NumeroVillaCreateDTO NumeroVilla { get; set; }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
