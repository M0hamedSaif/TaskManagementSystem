
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.APIs.Extensions;
using TaskManager.APIs.Middlewares;
using TaskManager.Repository.Data.Context;

namespace TaskManager.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Services
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters
                    .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });
            builder.Services.AddSwaggerServices();
            builder.Services.AddAuthServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin());
            });

            var app = builder.Build();

            //Seed Roles
            await app.SeedRolesAndMigrateAsync();

            //Middleware Pipeline
            app.UseMiddleware<ExceptionMiddleware>();


            if (app.Environment.IsDevelopment())
                app.UseSwaggerMiddlewares();

            app.UseStatusCodePagesWithReExecute("/error/{0}");


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
