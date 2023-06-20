using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO.NumeroVilla;
using MagicVilla_API.Modelos.DTO.Villa;
using MagicVilla_API.Modelos.Entidad;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> logger;
        private readonly IVillaRepositorio dbContextVilla;
        private readonly INumeroVillaRepositorio dbContextNumeroVilla;
        private readonly IMapper mapper;
        protected APIResponse response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger,
            IVillaRepositorio dbContextVilla,
            INumeroVillaRepositorio dbContextNumeroVilla,
            IMapper mapper
            )
        {
            this.logger = logger;
            this.dbContextVilla = dbContextVilla;
            this.dbContextNumeroVilla = dbContextNumeroVilla;
            this.mapper = mapper;
            response = new();
        }

        /// <summary>
        /// Mthod v2.0
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("2.0")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[]
            {
                "valor1","valor2"
            };
        }    
    }
}
