namespace realtorAPI.DTOs
{
    public class PropertyTraceCreateDto
    {
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public string IdProperty { get; set; } = string.Empty;
    }
}

