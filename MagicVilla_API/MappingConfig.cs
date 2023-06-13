using AutoMapper;
using MagicVilla_API.Modelos.DTO.NumeroVilla;
using MagicVilla_API.Modelos.DTO.Villa;
using MagicVilla_API.Modelos.Entidad;

namespace MagicVilla_API
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            ///villa          
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            ///Numero villa
            CreateMap<NumeroVilla, NumeroVillaDTO>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaCreateDTO>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateDTO>().ReverseMap();
        }
    }   
}
