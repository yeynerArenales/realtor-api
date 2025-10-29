# Realtor API

## Backend (.NET 9, ASP.NET Core, MongoDB)

- Simple layered architecture (Controllers → Services → MongoDB).
- Implemented improvements:
  - DataAnnotations validations in create DTOs.
  - Global error middleware with unified `ApiResponse<T>`.
  - Sorting and optional pagination in `GetProperties` (batch image resolution avoids N+1).
  - NUnit test project (validations, helpers, API responses).

### Endpoints
- `GET /api/properties?name=&address=&minPrice=&maxPrice=&page=&pageSize=&sortBy=Name|Price|Year&sortDir=asc|desc`
- `GET /api/properties/{id}`
- `POST /api/properties`
- `GET /api/owners`
- `GET /api/owners/{id}`
- `POST /api/owners`

### Standard response
```json
{
  "succeeded": true,
  "message": "...",
  "errors": [],
  "data": { }
}
```

### Key validations
- `PropertyCreateDto`: `Name`, `Address`, `CodeInternal`, `IdOwner` required. `Price ≥ 0`. `Year 1800-3000`.
- `OwnerCreateDto`: `Name`, `Address` required. `Photo` must be a valid URL.

### Error handling
- Exceptions:
  - `ArgumentException` → 400 with `ApiResponse.Error`.
  - `KeyNotFoundException` → 404.
  - Others → 500.
- Controllers can remove local `try/catch` blocks and rely on the middleware.

### Performance
- Suggested MongoDB indexes:
  - `properties(Name, Address)` for regex searches.
  - `properties(Price)` for price range queries.
  - `propertyimages(IdProperty, Enabled)` for image resolution.
- Batched image fetching to avoid N+1.
- Pagination is opt-in via `page` and/or `pageSize` (default disabled). Max `pageSize` enforced to 100.

### Running tests (NUnit)
- Project: `tests/realtorAPI.Tests`
- Includes tests for:
  - `ApiResponse<T>`
  - `ModelStateExtensions`
  - DTO validations

---

## Configuration
- `appsettings.json`:
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://...",
    "DatabaseName": "realtor"
  }
}
```

## Development
- Swagger enabled in Development at `/`.
- HTTPS enabled.

## Code standards
- Descriptive names (no cryptic abbreviations).
- Centralized error handling, early returns.
- Only add comments that provide non-trivial value.
