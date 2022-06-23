namespace CapacityPlanner.Models
{
    public record OccupancyReservationTrend
    {
        public string HotelCode { get; set; }
        public DateTime Date { get; set; }
        public int ReservationNumberEstimation { get; set; }

        public bool IsValid() => 
            Date.Date >= DateTime.Today 
            && ReservationNumberEstimation >= 0;
    }
}
