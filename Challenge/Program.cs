
using Challenge.Contracts;
using Challenge.Data;
using Challenge.Data.Models;
using Challenge.Filters;
using Challenge.Implementations;
using Challenge.Middleware;
using Challenge.Services;
using Microsoft.EntityFrameworkCore;

namespace Challenge
{
    public class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add configuration settings as needed
            builder.Configuration.AddJsonFile("appsettings.json");

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=app.db"));
            builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            // Add the custom exception filter globally.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>(); 
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Add middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseRouting();


            app.Run();
        }
    }
}