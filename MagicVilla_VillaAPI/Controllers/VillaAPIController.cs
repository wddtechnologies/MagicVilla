using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VilliaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult<VilliaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null) {
                return NotFound();
            }
            return Ok();
        }
        [HttpPost]
        public ActionResult<VilliaDTO> CreateVilla([FromBody] VilliaDTO villaDTO)
        {
            // Check if the villaDTO is null
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            // Check if a villa with the same name already exists
            //used StringComparison.OrdinalIgnoreCase instead of lower case
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.Equals(villaDTO.Name, StringComparison.OrdinalIgnoreCase)) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }

            // Validate that the ID is not set
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Generate a new ID and add the villa to the list
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault()?.Id + 1 ?? 1;
            VillaStore.villaList.Add(villaDTO);

            // Return a CreatedAtRoute response with the new villa details
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public ActionResult<VilliaDTO> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null) 
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public ActionResult UpdateVilla(int id, [FromBody] VilliaDTO villiaDTO)
        {
            if (villiaDTO == null || id != villiaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u =>u.Id == id);
            villa.Name = villiaDTO.Name;
            villa.Sqft = villiaDTO.Sqft;
            villa.Occupancy = villiaDTO.Occupancy;

            return NoContent ();



        }
    }
}
