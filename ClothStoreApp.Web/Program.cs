using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Data.Seeds;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Mappers;
using ClothStoreApp.Handler.Services;
using ClothStoreApp.Share.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

            // This .AddIdentity calls the .AddAuthentication() automatically, so we do not need
            // config the authentication service
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(builder =>
            {
                builder.User.RequireUniqueEmail = true;

                builder.SignIn.RequireConfirmedPhoneNumber = false;
                builder.SignIn.RequireConfirmedEmail = false;
                builder.SignIn.RequireConfirmedAccount = false;

                builder.Password.RequireDigit = true;
                builder.Password.RequireUppercase = true;
                builder.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            JwtSecurityConfiguration jwtConfig = new();
            builder.Configuration.GetSection("JwtSecurityConfiguration")
                .Bind(jwtConfig);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = jwtConfig.Audience,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.Configure<JwtSecurityConfiguration>(options =>
                builder.Configuration.GetSection("JwtSecurityConfiguration").Bind(options));

            // Register handler for Mediator broker
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Broker).Assembly));
            builder.Services.AddTransient<IBroker, Broker>();

            builder.Services.Configure<EmailConfiguration>(config =>
                builder.Configuration.GetSection(nameof(EmailConfiguration)).Bind(config));
            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddAutoMapper(config => config.AddMaps(typeof(MapperProfile).Assembly));

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

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specific the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClothStoreApi V1");
                c.RoutePrefix = string.Empty;   // Set Swagger UI at apps root
            });

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute("default", "/{controller=Home}/{action=Index}/{id?}");
            });

            // Seed data
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetService<ApplicationDbContext>();
                DataSeeder.Seed(db);
            }
           
            app.Run();

            
        }
    }
}