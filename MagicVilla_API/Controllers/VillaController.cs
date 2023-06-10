using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> logger;
        private readonly ApplicationDbContext context;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        /// <summary>
        /// Listado de villas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            logger.LogInformation("Obtener las villas");
            return Ok (context.Villas.ToList());
        }

        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK )] // Metodo documentado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDTO> GetVilla(int id)
        {
            // Si el id ingresado por el usuario es = a 0 retorna un BadRequest
            if (id == 0)
            {
                logger.LogError("Error al traer la villa con el id " + id);
                return BadRequest();
            }
               var villa = context.Villas.FirstOrDefault(x => x.Id == id);


            // Si el usuario no ingresa ningun id retorna un NotFound
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }
        /// <summary>
        /// Crear Villa
        /// </summary>
        /// <param name="villaDTO"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<VillaDTO> CrearVilla([FromBody]VillaDTO villaDTO) 
        {
            // Si el modelo no es valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // no ingresar nombre de villa repetidos
            if (context.Villas.FirstOrDefault(x => x.Nombre.ToLower() == villaDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Nombre existe", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }

            // si el usuario envia los campos vacios
            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            // Si el id > 0 error
            if(villaDTO.Id>0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa modelo = new()
            {
              //  Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                ImagenURL = villaDTO.ImagenURL,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad             
            };
            context.Villas.Add(modelo);
            context.SaveChanges();

            return CreatedAtRoute("GetVilla", new {id=villaDTO.Id}, villaDTO);
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
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO ==null || id!= villaDTO.Id)
            {
                return BadRequest();
            }

            Villa modelo = new()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                ImagenURL = villaDTO.ImagenURL,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad
            };

            context.Villas.Update(modelo);
            context.SaveChanges();

            return NoContent();
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
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> jsonPatch)
        {
            if (jsonPatch == null || id == 0)
            {
                return BadRequest();
            }
            // Buscamos el registro que se va a modificar

            var villa = context.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);

            VillaDTO villaDTO = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenURL = villa.ImagenURL,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };

            if (villa == null) return BadRequest();

            jsonPatch.ApplyTo(villaDTO, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // lo inverso

            Villa modelo = new()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                ImagenURL = villaDTO.ImagenURL,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad
            };

            context.Villas.Update(modelo);
            context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa=context.Villas.FirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            context.Villas.Remove(villa);
            context.SaveChanges();

            return NoContent();
        }
    }
}
