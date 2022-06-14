namespace CapacityPlanner.Models
{
    public record OccupancyReservationTrend
    {
        public DateTime Date { get; set; }
        public int EstimatedReservations { get; set; }
    }
}
