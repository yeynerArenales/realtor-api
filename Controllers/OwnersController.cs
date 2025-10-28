using Microsoft.AspNetCore.Mvc;
using realtorAPI.DTOs;
using realtorAPI.Services;
using realtorAPI.Helpers;

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
        public async Task<ActionResult<ApiResponse<List<OwnerResponseDto>>>> GetOwners()
        {
            try
            {
                var owners = await _ownerService.GetOwnersAsync();
                return Ok(ApiResponse<List<OwnerResponseDto>>.Success(owners, "Owners retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<OwnerResponseDto>>.Error("Error retrieving owners", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OwnerResponseDto>>> GetOwnerById(string id)
        {
            try
            {
                if (!MongoDB.Bson.ObjectId.TryParse(id, out _))
                {
                    return BadRequest(ApiResponse<OwnerResponseDto>.Error("Invalid Owner ID format"));
                }

                var owner = await _ownerService.GetOwnerByIdAsync(id);

                if (owner == null)
                {
                    return NotFound(ApiResponse<OwnerResponseDto>.Error($"Owner with id {id} not found"));
                }

                return Ok(ApiResponse<OwnerResponseDto>.Success(owner, "Owner retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<OwnerResponseDto>.Error("Error retrieving owner", ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OwnerResponseDto>>> CreateOwner([FromBody] OwnerCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.GetErrors();
                    return BadRequest(ApiResponse<OwnerResponseDto>.Error("Invalid model state", errors));
                }

                var owner = await _ownerService.CreateOwnerAsync(createDto);
                
                return CreatedAtAction(nameof(GetOwnerById), new { id = owner.Id }, 
                    ApiResponse<OwnerResponseDto>.Success(owner, "Owner created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<OwnerResponseDto>.Error("Error creating owner", ex.Message));
            }
        }
    }
}
