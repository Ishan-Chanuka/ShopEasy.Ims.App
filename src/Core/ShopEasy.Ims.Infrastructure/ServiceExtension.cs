using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShopEasy.Ims.Application.Services;
using ShopEasy.Ims.Domain.Models.DbModels;
using ShopEasy.Ims.Infrastructure.Context;
using ShopEasy.Ims.Infrastructure.Services;
using System.Text;

namespace ShopEasy.Ims.Infrastructure
{
    public static class ServiceExtension
    {
        public static void AddDatabaseExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
        }

        public static void AddRepositoriesExtension(this IServiceCollection services)
        {
            services.AddScoped<IProductsService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddTransient<ISeeder, Seeder>();
        }

        public static void AddAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["ApiSettings:Issuer"],
                        ValidAudience = configuration["ApiSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("ApiSettings:SecretKey").Value!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddSeedersExtension(this IServiceCollection services)
        {
            services.AddScoped<ISeeder, Seeder>();
        }
    }
}
