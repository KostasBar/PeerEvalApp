
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PeerEvalAppAPI.Configuration;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.Repositories;
using PeerEvalAppAPI.Services;
using Serilog;
using System.Reflection;

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

            builder.Services.AddScoped<IApplicationService, ApplicationService>();

            builder.Services.AddRepositories();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PeerEvalAppAPI", Version = "v1" });

                // Ensure the XML file for documenting your API is generated and set correctly
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200") 
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MyAllowSpecificOrigins");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
