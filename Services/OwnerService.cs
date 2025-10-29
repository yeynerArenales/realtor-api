using MongoDB.Driver;
using realtorAPI.Models;
using realtorAPI.DTOs;

namespace realtorAPI.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IMongoCollection<Owner> _owners;

        public OwnerService(IMongoDatabase database)
        {
            _owners = database.GetCollection<Owner>("owners");
        }

        public async Task<List<OwnerResponseDto>> GetOwnersAsync(int? page = null, int? pageSize = null)
        {
            var query = _owners.Find(_ => true);

            var shouldPaginate = page.HasValue || pageSize.HasValue;
            if (shouldPaginate)
            {
                var effectivePage = page.GetValueOrDefault(1);
                effectivePage = effectivePage <= 0 ? 1 : effectivePage;
                var effectivePageSize = pageSize.GetValueOrDefault(20);
                effectivePageSize = effectivePageSize <= 0 || effectivePageSize > 100 ? 20 : effectivePageSize;

                query = query.Skip((effectivePage - 1) * effectivePageSize).Limit(effectivePageSize);
            }

            var owners = await query.ToListAsync();
            
            return owners.Select(o => new OwnerResponseDto
            {
                Id = o.IdOwner,
                Name = o.Name,
                Address = o.Address,
                Photo = o.Photo,
                Birthday = o.Birthday
            }).ToList();
        }

        public async Task<OwnerResponseDto?> GetOwnerByIdAsync(string id)
        {
            var owner = await _owners.Find(o => o.IdOwner == id).FirstOrDefaultAsync();

            if (owner == null)
            {
                return null;
            }

            return new OwnerResponseDto
            {
                Id = owner.IdOwner,
                Name = owner.Name,
                Address = owner.Address,
                Photo = owner.Photo,
                Birthday = owner.Birthday
            };
        }

        public async Task<OwnerResponseDto> CreateOwnerAsync(OwnerCreateDto createDto)
        {
            var owner = new Owner
            {
                Name = createDto.Name,
                Address = createDto.Address,
                Photo = createDto.Photo,
                Birthday = createDto.Birthday
            };

            await _owners.InsertOneAsync(owner);

            return new OwnerResponseDto
            {
                Id = owner.IdOwner,
                Name = owner.Name,
                Address = owner.Address,
                Photo = owner.Photo,
                Birthday = owner.Birthday
            };
        }
    }
}

