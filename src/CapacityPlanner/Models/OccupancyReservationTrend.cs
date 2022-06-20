namespace CapacityPlanner.Models
{
    public record OccupancyReservationTrend
    {
        public string HotelCode { get; set; }
        public DateTime Date { get; set; }
        public int EstimatedReservations { get; set; }
    }
}
