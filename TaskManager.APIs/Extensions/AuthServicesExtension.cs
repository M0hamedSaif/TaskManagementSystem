using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TaskManager.Core.Entities.Identity;
using TaskManager.Repository.Data.Context;

namespace TaskManager.APIs.Extensions
{
    public static class AuthServicesExtension
    {
        public static IServiceCollection AddAuthServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Register Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
               .AddEntityFrameworkStores<TaskManagerDbContext>()
               .AddDefaultTokenProviders();


            // Register JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme; 
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = config["JWT:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JWT:Key"]!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });


            return services;
        }
    }
}
