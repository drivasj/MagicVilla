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

namespace MagicVilla_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
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
        /// Listado de villas
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                logger.LogInformation("Obtener Números villas");

                IEnumerable<NumeroVilla> villaList = await dbContextNumeroVilla.ObtenerTodos(incluirPropiedades: "Villa");

                response.Resultado = mapper.Map<IEnumerable<NumeroVillaDTO>>(villaList);
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
        /// GetVilla (id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id:int}", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                // Si el id ingresado por el usuario es = a 0 retorna un BadRequest
                if (id == 0)
                {
                    logger.LogError("Error al traer número villa con el id " + id);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsExitoso = false;
                    return BadRequest(response);
                }
                var villa = await dbContextNumeroVilla.Obtener(x => x.VillaNo == id);


                // Si el usuario no ingresa ningun id retorna un NotFound
                if (villa == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsExitoso = false;
                    return NotFound(response);
                }
                response.Resultado = mapper.Map<NumeroVillaDTO>(villa);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] NumeroVillaCreateDTO CreateDTO)
        {
            try
            {
                // Si el modelo no es valido
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // no ingresar nombre de villa repetidos
                if (await dbContextNumeroVilla.Obtener(x => x.VillaNo == CreateDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessasges", "La villa con ese nombre ya existe!");
                    return BadRequest(ModelState);
                }

                if (await dbContextVilla.Obtener(x => x.Id == CreateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessasges", "El id de la villa no existe!");
                    return BadRequest(ModelState);
                }

                // si el usuario envia los campos vacios
                if (CreateDTO == null)
                {
                    return BadRequest(CreateDTO);
                }


                NumeroVilla modelo = mapper.Map<NumeroVilla>(CreateDTO);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await dbContextNumeroVilla.Crear(modelo);
                response.Resultado = modelo;
                response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, response);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.VillaNo)
            {
                response.IsExitoso = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            if (await dbContextVilla.Obtener(x => x.Id == updateDTO.VillaId) == null)
            {
                ModelState.AddModelError("ErrorMessasges", "El id de la villa no existe!");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = mapper.Map<NumeroVilla>(updateDTO);

            await dbContextNumeroVilla.Actualizar(modelo);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialNumeroVilla(int id, JsonPatchDocument<NumeroVillaUpdateDTO> jsonPatch)
        {
            if (jsonPatch == null || id == 0)
            {
                return BadRequest();
            }
            // Buscamos el registro que se va a modificar

            var villa = await dbContextNumeroVilla.Obtener(x => x.VillaNo == id, tracked: false);

            NumeroVillaUpdateDTO villaUpdateDTO = mapper.Map<NumeroVillaUpdateDTO>(villa); // mapeo

            if (villa == null) return BadRequest();

            jsonPatch.ApplyTo(villaUpdateDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = mapper.Map<NumeroVilla>(villaUpdateDTO); // mapeo reverso

            await dbContextNumeroVilla.Actualizar(modelo);
            response.StatusCode = HttpStatusCode.NoContent;
            return Ok(response);
        }

        /// <summary>
        /// DELETE 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    response.IsExitoso = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                var villa = await dbContextNumeroVilla.Obtener(x => x.VillaNo == id);
                if (villa == null)
                {
                    response.IsExitoso = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                await dbContextNumeroVilla.Remover(villa);
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
