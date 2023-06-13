using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Config;

public class AuthConfig
{
    public static void Execute(WebApplicationBuilder builder)
    {
        builder.Services
        .AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetConnectionString("key")!);
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        builder.Services.AddAuthorization(
            opt =>
            {
                opt.AddPolicy("IT", policy => policy.RequireClaim("Role", "IT"));
                opt.AddPolicy("ENG", policy => policy.RequireClaim("Role", "ENG"));
            }
        );

    }
}