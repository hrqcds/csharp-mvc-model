using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Database;
using Repositories;
using Repositories.Implementations;

namespace Config;

public class BuilderConfig
{
    public static WebApplication Build(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services
            .AddSwaggerGen((options) =>
                {
                    SwaggerConfig.SwaggerConfigAuthentication(options);
                });

        builder.Services.AddDbContext<Context>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("db"))
        );

        builder.Services.AddScoped<IUserRepository, UserEntityRepository>();

        AuthConfig.Execute(builder);

        return builder.Build();
    }
}