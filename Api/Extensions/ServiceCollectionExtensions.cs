using System.Text;
using Api.Services;
using Application.Auth.Interfaces;
using Application.Auth.UseCases;
using Application.Common.Interfaces;
using Application.Electores.Interfaces;
using Application.Electores.UseCases;
using Application.Operadores.Interfaces;
using Application.Operadores.UseCases;
using Application.Tenants.Interfaces;
using Application.Roles;
using Application.Users.Interfaces;
using Application.Users.UseCases;
using Infrastructure.Auth;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        return services;
    }
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IElectorRepository, ElectorRepository>();
        services.AddScoped<IElectorConsultaRepository, ElectorConsultaRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IOperadorElectorRepository, OperadorElectorRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Login>();
        services.AddScoped<RefreshTokenUseCase>();
        services.AddScoped<GetOperators>();
        services.AddScoped<GetCoordinators>();
        services.AddScoped<RegisterUser>();
        services.AddScoped<GetElectorByNumeroCed>();
        services.AddScoped<GetEstadisticasConsulta>();
        services.AddScoped<ResetPassword>();
        services.AddScoped<AsignarElector>();
        services.AddScoped<GetElectoresDeOperador>();
        services.AddScoped<ActualizarElector>();
        services.AddScoped<RemoverElector>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                };
            });
        services.AddAuthorization();
        return services;
    }
}