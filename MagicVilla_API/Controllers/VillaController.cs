using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO.Villa;
using MagicVilla_API.Modelos.Entidad;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> logger;
        private readonly IVillaRepositorio context;
        private readonly IMapper mapper;
        protected APIResponse response;

        public VillaController(ILogger<VillaController> logger, 
            IVillaRepositorio context,
            IMapper mapper
            )
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
            response = new();
        }

        /// <summary>
        /// Listado de villas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                logger.LogInformation("Obtener las villas");

                IEnumerable<Villa> villaList = await context.ObtenerTodos();

                response.Resultado = mapper.Map<IEnumerable<VillaDTO>>(villaList);
                response.StatusCode = HttpStatusCode.OK;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.ErrorMessages = new  List<string> { ex.ToString() };
             
            }
            return response;

        }
        /// <summary>
        /// GetVilla (id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpGet("{id:int}", Name ="GetVilla")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task <ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                // Si el id ingresado por el usuario es = a 0 retorna un BadRequest
                if (id == 0)
                {
                    logger.LogError("Error al traer la villa con el id " + id);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsExitoso = false;
                    return BadRequest(response);
                }
                var villa = await context.Obtener(x => x.Id == id);


                // Si el usuario no ingresa ningun id retorna un NotFound
                if (villa == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsExitoso = false;
                    return NotFound(response);
                }
                response.Resultado = mapper.Map<VillaDTO>(villa);
                response.StatusCode = HttpStatusCode.OK;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
                
            }
            return response;
        }

        /// <summary>
        /// Crear Villa [POST]
        /// </summary>
        /// <param name="villaDTO"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<APIResponse>> CrearVilla([FromBody]VillaCreateDTO CreateDTO) 
        {
            try
            {
                // Si el modelo no es valido
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // no ingresar nombre de villa repetidos
                if (await context.Obtener(x => x.Nombre.ToLower() == CreateDTO.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessasges", "La villa con ese nombre ya existe!");
                    return BadRequest(ModelState);
                }

                // si el usuario envia los campos vacios
                if (CreateDTO == null)
                {
                    return BadRequest(CreateDTO);
                }


                Villa modelo = mapper.Map<Villa>(CreateDTO);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await context.Crear(modelo);
                response.Resultado = modelo;
                response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, response);
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return response;
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="id"></param>
        /// <param name="villaDTO"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id!= updateDTO.Id)
            {
                response.IsExitoso = false;
                response.StatusCode =HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            Villa modelo = mapper.Map<Villa>(updateDTO);

            await context.Actualizar(modelo);
            response.StatusCode = HttpStatusCode.NoContent;

            return Ok(response);
        }

        /// <summary>
        /// PATCH
        /// </summary>
        /// <param name="id"></param>
        /// <param name="villaDTO"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> jsonPatch)
        {
            if (jsonPatch == null || id == 0)
            {
                return BadRequest();
            }
            // Buscamos el registro que se va a modificar

            var villa = await context.Obtener(x => x.Id == id, tracked:false);

            VillaUpdateDTO villaUpdateDTO = mapper.Map<VillaUpdateDTO>(villa); // mapeo
    
            if (villa == null) return BadRequest();

            jsonPatch.ApplyTo(villaUpdateDTO, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo =  mapper.Map<Villa>(villaUpdateDTO); // mapeo reverso

            await context.Actualizar(modelo);
            response.StatusCode = HttpStatusCode.NoContent;
            return Ok(response);
        }

        /// <summary>
        /// DELETE 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpDelete("{id:int}")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    response.IsExitoso = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                var villa = await context.Obtener(x => x.Id == id);
                if (villa == null)
                {
                    response.IsExitoso = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                await context.Remover(villa);
                return NoContent();
            }
            catch (Exception ex)
            {
                response.IsExitoso = false;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(response);
        }
    }
}
