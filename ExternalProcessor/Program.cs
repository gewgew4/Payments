
using EasyCronJob.Core;
using ExternalProcessor.Services;

namespace ExternalProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CronJob
            builder.Services.ApplyResulation<CronJob>(options =>
            {
                options.CronExpression = "*/10 * * * * *";
                options.TimeZoneInfo = TimeZoneInfo.Local;
                options.CronFormat = Cronos.CronFormat.IncludeSeconds;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
