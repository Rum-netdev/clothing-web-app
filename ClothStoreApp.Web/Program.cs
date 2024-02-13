using ClothStoreApp.Data;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ClothStoreApp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlOption =>
                {
                    sqlOption.EnableRetryOnFailure(3);
                    sqlOption.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString());
                });
                option.EnableDetailedErrors();
                option.EnableSensitiveDataLogging();
            });

            // Register handler for Mediator broker
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Broker).Assembly));
            builder.Services.AddTransient<IBroker, Broker>();

            builder.Services.AddAutoMapper(config => config.AddProfile(new MapperProfile()));

            builder.Services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo() { Title = "ClothStoreApi", Version = "v1" });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            // Enable middleware to serve generated swagger as a JSON endpoint
            app.UseSwagger();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClothStoreApi V1");
                c.RoutePrefix = string.Empty;   // Set Swagger UI at apps root
            });

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute("default", "/{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specific the Swagger JSON endpoint
            app.Run();
        }
    }
}