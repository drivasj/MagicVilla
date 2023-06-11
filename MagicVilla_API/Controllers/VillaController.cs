using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.DTO.Villa;
using MagicVilla_API.Modelos.Entidad;
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
        private readonly IMapper mapper;

        public VillaController(ILogger<VillaController> logger, 
            ApplicationDbContext context,
            IMapper mapper
            )
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Listado de villas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task <ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            logger.LogInformation("Obtener las villas");

            IEnumerable<Villa> villaList = await context.Villas.ToListAsync();

            return Ok (mapper.Map<IEnumerable<VillaDTO>>(villaList));
        }
        /// <summary>
        /// GetVilla (id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task <ActionResult<VillaDTO>> GetVilla(int id)
        {
            // Si el id ingresado por el usuario es = a 0 retorna un BadRequest
            if (id == 0)
            {
                logger.LogError("Error al traer la villa con el id " + id);
                return BadRequest();
            }
               var villa = await context.Villas.FirstOrDefaultAsync(x => x.Id == id);


            // Si el usuario no ingresa ningun id retorna un NotFound
            if (villa == null)
            {
                return NotFound();
            }
            var query = mapper.Map<VillaDTO>(villa);
            return Ok(query);
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
        public async Task <ActionResult<VillaDTO>> CrearVilla([FromBody]VillaCreateDTO CreateDTO) 
        {
            // Si el modelo no es valido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // no ingresar nombre de villa repetidos
            if (await context.Villas.FirstOrDefaultAsync(x => x.Nombre.ToLower() == CreateDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Nombre existe", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }

            // si el usuario envia los campos vacios
            if(CreateDTO == null)
            {
                return BadRequest(CreateDTO);
            }

            //Villa modelo = new()
            //{
            //  //  Id = villaDTO.Id,
            //    Nombre = villaDTO.Nombre,
            //    Detalle = villaDTO.Detalle,
            //    ImagenURL = villaDTO.ImagenURL,
            //    Ocupantes = villaDTO.Ocupantes,
            //    Tarifa = villaDTO.Tarifa,
            //    MetrosCuadrados = villaDTO.MetrosCuadrados,
            //    Amenidad = villaDTO.Amenidad             
            //};

            Villa modelo = mapper.Map<Villa>(CreateDTO);

            await context.Villas.AddAsync(modelo);
            await context.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new {id=modelo.Id}, modelo);
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
        public async Task <IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id!= updateDTO.Id)
            {
                return BadRequest();
            }

            //Villa modelo = new()
            //{
            //    Id = villaDTO.Id,
            //    Nombre = villaDTO.Nombre,
            //    Detalle = villaDTO.Detalle,
            //    ImagenURL = villaDTO.ImagenURL,
            //    Ocupantes = villaDTO.Ocupantes,
            //    Tarifa = villaDTO.Tarifa,
            //    MetrosCuadrados = villaDTO.MetrosCuadrados,
            //    Amenidad = villaDTO.Amenidad
            //};

            Villa modelo = mapper.Map<Villa>(updateDTO);

            context.Villas.Update(modelo);
            await  context.SaveChangesAsync();
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
        public async Task <IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> jsonPatch)
        {
            if (jsonPatch == null || id == 0)
            {
                return BadRequest();
            }
            // Buscamos el registro que se va a modificar

            var villa = await context.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            VillaUpdateDTO villaUpdateDTO = mapper.Map<VillaUpdateDTO>(villa); // mapeo
    
            if (villa == null) return BadRequest();

            jsonPatch.ApplyTo(villaUpdateDTO, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo =  mapper.Map<Villa>(villaUpdateDTO); // mapeo reverso

            context.Villas.Update(modelo);
            await context.SaveChangesAsync();

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

        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var  villa= await context.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            context.Villas.Remove(villa);
           await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
