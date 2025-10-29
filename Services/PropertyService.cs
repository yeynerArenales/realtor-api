using MongoDB.Driver;
using realtorAPI.Models;
using realtorAPI.DTOs;

namespace realtorAPI.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IMongoCollection<Property> _properties;
        private readonly IMongoCollection<PropertyImage> _propertyImages;
        private readonly IMongoCollection<Owner> _owners;

        public PropertyService(IMongoDatabase database)
        {
            _properties = database.GetCollection<Property>("properties");
            _propertyImages = database.GetCollection<PropertyImage>("propertyimages");
            _owners = database.GetCollection<Owner>("owners");
        }

        public async Task<List<PropertyResponseDto>> GetPropertiesAsync(PropertyFilterDto filter)
        {
            var filterBuilder = Builders<Property>.Filter;

            var filters = new List<FilterDefinition<Property>>();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                filters.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i")));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                filters.Add(filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(filter.Address, "i")));
            }

            if (filter.MinPrice.HasValue)
            {
                filters.Add(filterBuilder.Gte(p => p.Price, filter.MinPrice.Value));
            }

            if (filter.MaxPrice.HasValue)
            {
                filters.Add(filterBuilder.Lte(p => p.Price, filter.MaxPrice.Value));
            }

            var combinedFilter = filters.Count > 0 
                ? filterBuilder.And(filters) 
                : filterBuilder.Empty;

            var sortBuilder = Builders<Property>.Sort;
            SortDefinition<Property> sort = filter.SortBy?.ToLower() switch
            {
                "price" => (filter.SortDir?.ToLower() == "desc" ? sortBuilder.Descending(p => p.Price) : sortBuilder.Ascending(p => p.Price)),
                "year" => (filter.SortDir?.ToLower() == "desc" ? sortBuilder.Descending(p => p.Year) : sortBuilder.Ascending(p => p.Year)),
                _ => (filter.SortDir?.ToLower() == "desc" ? sortBuilder.Descending(p => p.Name) : sortBuilder.Ascending(p => p.Name))
            };

            var shouldPaginate = filter.Page.HasValue || filter.PageSize.HasValue;
            List<Property> properties;
            if (shouldPaginate)
            {
                var page = (filter.Page ?? 1);
                page = page <= 0 ? 1 : page;
                var pageSize = (filter.PageSize ?? 20);
                pageSize = pageSize <= 0 || pageSize > 100 ? 20 : pageSize;
                var skip = (page - 1) * pageSize;

                properties = await _properties
                    .Find(combinedFilter)
                    .Sort(sort)
                    .Skip(skip)
                    .Limit(pageSize)
                    .ToListAsync();
            }
            else
            {
                properties = await _properties
                    .Find(combinedFilter)
                    .Sort(sort)
                    .ToListAsync();
            }

            if (properties.Count == 0)
            {
                return new List<PropertyResponseDto>();
            }

            var propertyIds = properties.Select(p => p.IdProperty).ToList();
            var images = await _propertyImages
                .Find(Builders<PropertyImage>.Filter.And(
                    Builders<PropertyImage>.Filter.In(pi => pi.IdProperty, propertyIds),
                    Builders<PropertyImage>.Filter.Eq(pi => pi.Enabled, true)))
                .ToListAsync();

            var propertyIdToImage = images
                .GroupBy(i => i.IdProperty)
                .ToDictionary(g => g.Key, g => g.First().File);

            var result = properties.Select(property => new PropertyResponseDto
            {
                Id = property.IdProperty,
                OwnerId = property.IdOwner,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                Image = propertyIdToImage.TryGetValue(property.IdProperty, out var file) ? file : null
            }).ToList();

            return result;
        }

        public async Task<PropertyResponseDto?> GetPropertyByIdAsync(string id)
        {
            var property = await _properties.Find(p => p.IdProperty == id).FirstOrDefaultAsync();

            if (property == null)
            {
                return null;
            }

            var image = await _propertyImages
                .Find(pi => pi.IdProperty == property.IdProperty && pi.Enabled == true)
                .FirstOrDefaultAsync();

            return new PropertyResponseDto
            {
                Id = property.IdProperty,
                OwnerId = property.IdOwner,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                Image = image?.File
            };
        }

        public async Task<PropertyResponseDto?> CreatePropertyAsync(PropertyCreateDto createDto)
        {
            if (!MongoDB.Bson.ObjectId.TryParse(createDto.IdOwner, out _))
            {
                throw new ArgumentException("Invalid Owner ID format. Must be a valid 24-digit hexadecimal string.");
            }

            var owner = await _owners.Find(o => o.IdOwner == createDto.IdOwner).FirstOrDefaultAsync();
            
            if (owner == null)
            {
                return null;
            }

            var property = new Property
            {
                Name = createDto.Name,
                Address = createDto.Address,
                Price = createDto.Price,
                CodeInternal = createDto.CodeInternal,
                Year = createDto.Year,
                IdOwner = createDto.IdOwner
            };

            await _properties.InsertOneAsync(property);

            return new PropertyResponseDto
            {
                Id = property.IdProperty,
                OwnerId = property.IdOwner,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                Image = null
            };
        }
    }
}

