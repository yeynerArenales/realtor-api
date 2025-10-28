using Microsoft.AspNetCore.Mvc;
using realtorAPI.DTOs;
using realtorAPI.Services;

namespace realtorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyService _propertyService;

        public PropertiesController(PropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PropertyResponseDto>), 200)]
        public async Task<ActionResult<List<PropertyResponseDto>>> GetProperties([FromQuery] PropertyFilterDto filter)
        {
            try
            {
                var properties = await _propertyService.GetPropertiesAsync(filter);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving properties", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropertyResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PropertyResponseDto>> GetPropertyById(string id)
        {
            try
            {
                var property = await _propertyService.GetPropertyByIdAsync(id);

                if (property == null)
                {
                    return NotFound(new { message = $"Property with id {id} not found" });
                }

                return Ok(property);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving property", error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(PropertyResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PropertyResponseDto>> CreateProperty([FromBody] PropertyCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var property = await _propertyService.CreatePropertyAsync(createDto);

                if (property == null)
                {
                    return NotFound(new { message = "Owner not found" });
                }

                return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, property);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating property", error = ex.Message });
            }
        }
    }
}

