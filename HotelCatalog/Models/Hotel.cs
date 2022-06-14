namespace HotelCatalog.Models
{
    public record Hotel
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public bool HasSwimmingPool { get; set; }

    }
}
