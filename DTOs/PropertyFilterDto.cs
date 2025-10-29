namespace realtorAPI.DTOs
{
    public class PropertyFilterDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public string? SortBy { get; set; } = "Name";
        public string? SortDir { get; set; } = "asc";
    }
}

