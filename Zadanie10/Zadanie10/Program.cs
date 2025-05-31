using Microsoft.EntityFrameworkCore;
using Zadanie10.Data;

using Zadanie10.Services;

namespace Zadanie10;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        builder.Services.AddDbContext<MasterContext>(opt =>
{
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
        });
        
        builder.Services.AddScoped<IDbService, DbService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}