namespace CapacityPlanner.Models
{
    public record OccupancyHistoricalForecast
    {
        public string HotelCode { get; set; }

        public DateTime Date { get; set; }
        /// <summary>
        /// 0 to 10, where 0 is empty and 10 is full
        /// </summary>
        public int HistoricalLevel { get; set; }

    }
}
