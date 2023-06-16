using AutoMapper;
using Magic_Villa_Web.Models.DTO.NumeroVilla;
using Magic_Villa_Web.Models.DTO.Villa;

namespace Magic_Villa_Web
{
    public class MappingConfig: Profile 
    {
        public MappingConfig()
        {
            //Villa
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            //Numero Villa
            CreateMap<NumeroVillaDTO, NumeroVillaCreateDTO>().ReverseMap();
            CreateMap<NumeroVillaDTO, NumeroVillaUpdateDTO>().ReverseMap();

        }
    }
}
