namespace HelpDesk.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddHttpContextAccessor();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
