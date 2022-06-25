using HotelCatalog.Services;

namespace HotelCatalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                 .Build();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDaprClient();
#if DEBUG
            builder.Services.AddDaprSidekick(configuration);
#endif
            builder.Services.AddSingleton<HotelCatalogService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}