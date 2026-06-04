
using gsst.Interfaces;
using gsst.Model.OrderProcessors;
using gsst.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PetrolAPI.Services;
using Scalar.AspNetCore;
using System.Text;

namespace PetrolAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtSettings")["Secret"]);

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddControllers();
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new();
                    document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>()
                    {
                        ["Bearer"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "JWT Authorization header using the Bearer scheme."
                        }
                    };

                    document.Security = [
                        new OpenApiSecurityRequirement{
                            {new OpenApiSecuritySchemeReference("Bearer"), new List<string>()}
                        }
                        ];

                    return Task.CompletedTask;
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));


            builder.Services.AddScoped<IApiCustomerAuthService, ApiCustomerAuthService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddSingleton<SettingsService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, gsst.Services.AuthService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IBonusService, BonusService>();
            builder.Services.AddScoped<IOrderProcessor, FuelOrderProcessor>();
            builder.Services.AddScoped<IGoodsService, GoodsService>();
            builder.Services.AddScoped<IFuelTypeService, FuelTypeService>();
            builder.Services.AddScoped<ITanksService, TanksService>();
            builder.Services.AddScoped<IPumpService, PumpService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder => builder
                        .WithOrigins("http://localhost:5173", "http://localhost:5174")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");
            app.UseAuthentication();
            app.UseAuthorization();


            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();

                // Seed Test Users
                var adminUser = dbContext.Users.FirstOrDefault(u => u.Username == "admin");
                if (adminUser == null)
                {
                    dbContext.Users.Add(new gsst.Model.User.User { FullName = "Test Admin", PhoneNumber = "+48000000001", Username = "admin", Password = "password", Role = "Admin" });
                }
                else
                {
                    adminUser.Role = "Admin"; // Force Admin role back if changed
                    adminUser.FullName = "System Administrator";
                    adminUser.Password = "admin";
                }

                if (!dbContext.Users.Any(u => u.Username == "manager"))
                {
                    dbContext.Users.Add(new gsst.Model.User.User { FullName = "Test Manager", PhoneNumber = "+48000000002", Username = "manager", Password = "password", Role = "Manager" });
                }
                if (!dbContext.Users.Any(u => u.Username == "client"))
                {
                    var client = new gsst.Model.User.User { FullName = "Test Client", PhoneNumber = "+48000000003", Username = "client", Password = "password", Role = "Client" };
                    dbContext.Users.Add(client);
                    dbContext.SaveChanges(); // Need ID for bonus card
                    
                    dbContext.BonusCards.Add(new gsst.Model.BonusCard { UserId = client.Id, ClientName = client.FullName, Barcode = "TEST1234", BonusBalance = 100 });
                }
                dbContext.SaveChanges();

                var clientUser = dbContext.Users.FirstOrDefault(u => u.Username == "client");
                if (clientUser != null && !dbContext.PaymentMethods.Any(p => p.UserId == clientUser.Id && p.Type == "Crypto"))
                {
                    dbContext.PaymentMethods.Add(new gsst.Model.User.PaymentMethod { UserId = clientUser.Id, Type = "Crypto", Details = "Wallet: 0x71C7656EC7ab88b098defB751B7401B5f6d8976F", IsDefault = false });
                    dbContext.PaymentMethods.Add(new gsst.Model.User.PaymentMethod { UserId = clientUser.Id, Type = "Crypto", Details = "Wallet: bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh", IsDefault = false });
                    dbContext.SaveChanges();
                }
            }



            app.MapControllers();

            app.Run();
        }
    }
}
