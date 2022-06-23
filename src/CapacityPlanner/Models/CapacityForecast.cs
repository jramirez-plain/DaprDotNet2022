namespace CapacityPlanner.Models
{
    public record CapacityForecast(string HotelCode, DateTime Date, double OccupancyPercentage, double ConfidenceRate)
    {
        public static CapacityForecast Default(string hotelCode, DateTime date) => new(hotelCode, date, 0.5, 0.0);

        public bool IsValid() => 
            Date.Date >= DateTime.Today
            && (OccupancyPercentage >= 0.0 && OccupancyPercentage <= 1.0)
            && (ConfidenceRate >= 0.0 && ConfidenceRate <= 1.0);
    }
}
