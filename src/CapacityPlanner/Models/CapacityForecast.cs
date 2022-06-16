namespace CapacityPlanner.Models
{
    public record CapacityForecast(double OccupancyPercentage, double ConfidenceRate)
    {
        public static CapacityForecast Default() => new(0.5, 0.0);
    }
}
