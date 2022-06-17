namespace CapacityPlanner.Models
{
    public record CapacityForecast(string HotelCode, double OccupancyPercentage, double ConfidenceRate)
    {
        public static CapacityForecast Default(string hotelCode) => new(hotelCode, 0.5, 0.0);
    }
}
