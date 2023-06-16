using Magic_Villa_Web.Models.DTO.NumeroVilla;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magic_Villa_Web.Models.ViewModel
{
    public class NumeroVillaUpdateViewModel
    {
        public NumeroVillaUpdateViewModel()
        {
            NumeroVilla = new NumeroVillaUpdateDTO();
        }

        public NumeroVillaUpdateDTO NumeroVilla { get; set; }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
