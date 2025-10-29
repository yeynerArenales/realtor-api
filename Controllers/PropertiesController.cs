using Microsoft.AspNetCore.Mvc;
using realtorAPI.DTOs;
using realtorAPI.Services;
using realtorAPI.Helpers;

namespace realtorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PropertyResponseDto>>>> GetProperties([FromQuery] PropertyFilterDto filter)
        {
            try
            {
                var properties = await _propertyService.GetPropertiesAsync(filter);
                return Ok(ApiResponse<List<PropertyResponseDto>>.Success(properties, "Properties retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<PropertyResponseDto>>.Error("Error retrieving properties", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PropertyResponseDto>>> GetPropertyById(string id)
        {
            try
            {
                var property = await _propertyService.GetPropertyByIdAsync(id);

                if (property == null)
                {
                    return NotFound(ApiResponse<PropertyResponseDto>.Error($"Property with id {id} not found"));
                }

                return Ok(ApiResponse<PropertyResponseDto>.Success(property, "Property retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PropertyResponseDto>.Error("Error retrieving property", ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PropertyResponseDto>>> CreateProperty([FromBody] PropertyCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.GetErrors();
                    return BadRequest(ApiResponse<PropertyResponseDto>.Error("Invalid model state", errors));
                }

                var property = await _propertyService.CreatePropertyAsync(createDto);

                if (property == null)
                {
                    return NotFound(ApiResponse<PropertyResponseDto>.Error("Owner not found"));
                }

                return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, 
                    ApiResponse<PropertyResponseDto>.Success(property, "Property created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<PropertyResponseDto>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PropertyResponseDto>.Error("Error creating property", ex.Message));
            }
        }
    }
}
