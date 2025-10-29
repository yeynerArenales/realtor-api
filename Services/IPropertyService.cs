using realtorAPI.DTOs;

namespace realtorAPI.Services
{
    public interface IPropertyService
    {
        Task<List<PropertyResponseDto>> GetPropertiesAsync(PropertyFilterDto filter);
        Task<PropertyResponseDto?> GetPropertyByIdAsync(string id);
        Task<PropertyResponseDto?> CreatePropertyAsync(PropertyCreateDto createDto);
    }
}


