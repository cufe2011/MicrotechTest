
using Microsoft.EntityFrameworkCore;
using System;
using TestDevWebApi.Data;
using TestDevWebApi.Services;

namespace TestDevWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);




            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()  // Allow all origins (you can restrict this to specific domains)
                          .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, etc.)
                          .AllowAnyHeader(); // Allow any headers
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddDbContext<TestDevDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TestDevConnectionString")));

            var app = builder.Build();

           

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
