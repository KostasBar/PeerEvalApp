
using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Configuration;
using PeerEvalAppAPI.Data;
using Serilog;

namespace PeerEvalAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DB Context
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PeerEvalAppDbContext>(options => options.UseSqlServer(connString));

            // Add Serilog Configuration
            builder.Host.UseSerilog(
                (context, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration);
                }
             );

            // Add AutoMapper Config Class
            builder.Services.AddAutoMapper(typeof(MapperConfig));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
