namespace HotelCatalog.Models
{
    public record Hotel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double BaseRate { get; set; }
        public int Rating { get; set; }
        public int RoomNumber { get; set; }
        public bool? HasSwimmingPool { get; set; }
        public bool? HasWifi { get; set; }
        public bool? HasParking { get; set; }
        public bool? Has24hoursFrontDesk { get; set; }
        public bool? AllowsLateChecking { get; set; }

    }
}
