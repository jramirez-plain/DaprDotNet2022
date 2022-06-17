namespace CapacityPlanner.Models
{
    public record CapacityForecast(string HotelCode, DateTime Date, double OccupancyPercentage, double ConfidenceRate)
    {
        public static CapacityForecast Default(string hotelCode, DateTime date) => new(hotelCode, date, 0.5, 0.0);
    }
}
