using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        
        public VillaAPIController(ApplicationDbContext db) 
        {
            
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
           
            return Ok(_db.Villas.ToList());
        }
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null) {
                return NotFound();
            }
            //add in the villa to return the villa by id
            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {

            // Check if a villa with the same name already exists
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }

            // Check if the villaDTO is null
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            // Validate that the ID is not set
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Generate a new ID and add the villa to the list
            //_db.Villas.Add(_db.Villas.FirstOrDefault(u => u.Id == villaDTO.Id));
    
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageURL = villaDTO.ImageURL,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft

            };
            _db.Villas.Add(model);
            _db.SaveChanges();

            // Return a CreatedAtRoute response with the new villa details
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public ActionResult<VillaDTO> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null) 
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            //added save changes
            _db.SaveChanges();
            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public ActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageURL = villaDTO.ImageURL,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();

            //return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);

        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO>patchDTO)
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            VillaDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImageURL = villa.ImageURL,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft

            };

            if (villa == null)
            {
                return BadRequest();
            }

            patchDTO.ApplyTo(villaDTO, ModelState);
            //fix this section from set being private 
            Villa model = new Villa()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImageURL = villa.ImageURL,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft

            };
            _db.Villas.Update(villa);
            _db.SaveChanges();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
