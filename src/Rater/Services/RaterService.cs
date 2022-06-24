using Dapr.Client;
using Rater.Models;
using System.Text;

namespace Rater.Services
{
    public class RaterService
    {
        public async Task GenerateRates(HotelInfo hotel, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var rates = await CalculateRates(hotel, from, to, cancellationToken);
            await SendRatesMail(hotel, rates);

        }

        private async Task<IEnumerable<Rate>> CalculateRates(HotelInfo hotel, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var rates = new List<Rate>();
            for (var current = from; current <= to; current = current.AddDays(1))
            {
                rates.Add(await CalculateRate(hotel, current, cancellationToken));
            }
            return rates;
        }

        private async Task<Rate> CalculateRate(HotelInfo hotel, DateTime date, CancellationToken cancellationToken)
        {
            var capacity = await GetHotelCapacityForecast(hotel.Code, date, cancellationToken);

            var baseRate = hotel.BaseRate;
            var desviation = capacity.OccupancyPercentage - 0.5;
            var desviationMultiplier = desviation / 2;
            var confidenceRate = capacity.ConfidenceRate;
            var confidenceMultiplier = Math.Sqrt(confidenceRate);
            var rate = baseRate + (baseRate * desviationMultiplier * confidenceMultiplier);
            return new Rate(date, rate);
        }

        public Task<IEnumerable<HotelInfo>> GetHotels(string countryCode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<HotelInfo> GetHotel(string hotelCode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Task<CapacityForecast> GetHotelCapacityForecast(string hotelCode, DateTime date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task SendRatesMail(HotelInfo hotel, IEnumerable<Rate> rates)
        {
            const string from = "jramirez@plainconcepts.com";
            var body = GenerateMailBody(hotel, rates);
            await SendNotification(from, hotel.Email, "Rates", body);
        }

        private string GenerateMailBody(HotelInfo hotel, IEnumerable<Rate> rates)
        {
            var textBuilder = new StringBuilder();
            textBuilder.Append($"Rates for hotel {hotel.Name}<br/>");
            textBuilder.Append("<ul>");
            foreach (var rate in rates)
            {
                textBuilder.Append($"<li> {rate.Date.ToString("d")}: {rate.Price.ToString("C")}€ </li>");
            }
            textBuilder.Append("</ul>");
            return textBuilder.ToString();
        }

        private async Task SendNotification(string from, string to, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}
