using realtorAPI.DTOs;

namespace realtorAPI.Services
{
    public interface IOwnerService
    {
        Task<List<OwnerResponseDto>> GetOwnersAsync(int? page = null, int? pageSize = null);
        Task<OwnerResponseDto?> GetOwnerByIdAsync(string id);
        Task<OwnerResponseDto> CreateOwnerAsync(OwnerCreateDto createDto);
    }
}


