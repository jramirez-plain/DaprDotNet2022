namespace HotelCatalog.Models
{
    public record Apartment: Office
    {
        public int RoomNumber { get; set; }
        public bool HasKitchen { get; set; }
    }
}
