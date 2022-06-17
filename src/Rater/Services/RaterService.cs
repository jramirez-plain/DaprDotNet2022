using Dapr.Client;
using Rater.Models;
using System.Text;

namespace Rater.Services
{
    public class RaterService
    {
        private readonly DaprClient _daprClient;
        public RaterService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task GenerateRates(HotelInfo hotel, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var rates = await CalculateRates(hotel, from, to, cancellationToken);
            await SendRatesMail(hotel, rates);

        }

        private async Task<IEnumerable<Rate>> CalculateRates(HotelInfo hotel, DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var rates = new List<Rate>();
            for (var current = from; current <= to; current.AddDays(1))
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
            const string CATALOG = "catalog";
            const string CATALOG_METHOD = "hotels";
            return _daprClient.InvokeMethodAsync<IEnumerable<HotelInfo>>(
                HttpMethod.Get,
                CATALOG,
                $"{CATALOG_METHOD}?countryCode={countryCode}",
                cancellationToken);
        }

        public Task<HotelInfo> GetHotel(string hotelCode, CancellationToken cancellationToken)
        {
            const string CATALOG = "catalog";
            const string CATALOG_METHOD = "hotels";
            return _daprClient.InvokeMethodAsync<HotelInfo>(
                HttpMethod.Get,
                CATALOG,
                $"{CATALOG_METHOD}/{hotelCode}",
                cancellationToken);
        }

        private Task<CapacityForecast> GetHotelCapacityForecast(string hotelCode, DateTime date, CancellationToken cancellationToken)
        {
            const string CATALOG = "capacityplanner";
            return _daprClient.InvokeMethodAsync<CapacityForecast>(
                HttpMethod.Get,
                CATALOG,
                $"hotels/{hotelCode}/capacityforecasts/{date}",
                cancellationToken);
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
            textBuilder.AppendLine($"Rates for hotel {hotel.Name}");
            foreach (var rate in rates)
            {
                textBuilder.AppendLine($"Date {rate.Date}: {rate.Price}€");
            }
            return textBuilder.ToString();
        }

        private async Task SendNotification(string from, string to, string subject, string body)
        {
            string BINDING_NAME = "sendgrid";
            string BINDING_OPERATION = "create";
            var metadata = new Dictionary<string, string>()
            {
                ["emailTo"] = from,
                ["emailFrom"] = to,
                ["subject"] = subject,
            };
            try
            {
                await _daprClient.InvokeBindingAsync(BINDING_NAME, BINDING_OPERATION, body, metadata);
            }
            catch (Exception ex)
            {
            }

        }
    }
}
