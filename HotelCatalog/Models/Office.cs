namespace HotelCatalog.Models
{
    public record Office
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double BaseRate { get; set; }
    }
}
