using FluentValidation;
using HelpDesk.API.Exceptions;
using HelpDesk.Application.Common.Behaviors;
using HelpDesk.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HelpDesk.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDepenencies(this IServiceCollection services)
    {
        services.AddAuth();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        SerilogConfig();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((options, jwtOptions) =>
            {
                var jwt = jwtOptions.Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.SecretKey)),

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    public static void SerilogConfig()
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
            .CreateLogger();
    }
}
