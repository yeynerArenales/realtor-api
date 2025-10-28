using Microsoft.AspNetCore.Mvc;
using realtorAPI.DTOs;
using realtorAPI.Services;

namespace realtorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly OwnerService _ownerService;

        public OwnersController(OwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OwnerResponseDto>>> GetOwners()
        {
            try
            {
                var owners = await _ownerService.GetOwnersAsync();
                return Ok(owners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving owners", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerResponseDto>> GetOwnerById(string id)
        {
            try
            {
                if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
                {
                    return BadRequest(new { message = "Invalid Owner ID format" });
                }

                var owner = await _ownerService.GetOwnerByIdAsync(id);

                if (owner == null)
                {
                    return NotFound(new { message = $"Owner with id {id} not found" });
                }

                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving owner", error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(OwnerResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<OwnerResponseDto>> CreateOwner([FromBody] OwnerCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var owner = await _ownerService.CreateOwnerAsync(createDto);
                
                return CreatedAtAction(nameof(GetOwnerById), new { id = owner.Id }, owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating owner", error = ex.Message });
            }
        }
    }
}

