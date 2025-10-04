using Authen.Application.Abstractions;
using Authen.Infrastructure.Data;
using Authen.Infrastructure.Services;
using Common.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authen.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddAuthDbContext(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddDbContext<AuthenIdentityDbContext>(o =>
                o.UseNpgsql(cfg.GetConnectionString("DefaultDatabase"))); // or UseNpgsql/UseSqlite

            services
                .AddIdentityCore<Common.Domain.Entities.User>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequiredLength = 8;
                    opt.Lockout.MaxFailedAccessAttempts = 5;
                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                .AddRoles<Common.Domain.Entities.Role>()
                .AddEntityFrameworkStores<AuthenIdentityDbContext>()
                .AddDefaultTokenProviders(); // email confirmation, reset, 2FA
        }
        public static void AddAuthenProvider(this IServiceCollection services, IConfiguration cfg)
        {
            var key = Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!);
            var issuer = cfg["Jwt:Issuer"]!;
            var audience = cfg["Jwt:Audience"]!;
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // default = JWT
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services.AddAuthorization();
        }
    }
}
