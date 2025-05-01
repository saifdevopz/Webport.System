using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Infrastructure.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddOptions<JwtOptions>()
            .BindConfiguration("JwtOptions")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                JwtOptions jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value;

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddAuthorization();

        return services;
    }
}
