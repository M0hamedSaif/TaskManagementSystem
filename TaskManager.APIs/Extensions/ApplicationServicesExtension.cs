using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.APIs.Errors;
using TaskManager.APIs.Helpers;
using TaskManager.Core;
using TaskManager.Core.Services.Contract;
using TaskManager.Repository.Data.Context;
using TaskManager.Repository.UnitOfWork;
using TaskManager.Service.Services;

namespace TaskManager.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Register DbContext
            services.AddDbContext<TaskManagerDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            // Register App Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // Override default validation error response shape
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(p => p.Value!.Errors.Count > 0)
                        .SelectMany(p => p.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(new ApiValidationResponse
                    {
                        Errors = errors
                    });
                };
            });
            

            return services;
        }
    }
}
