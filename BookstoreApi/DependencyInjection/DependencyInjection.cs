using System.Reflection;
using System.Text;
using BookstoreApi.Data;
using BookstoreApi.Models;
using BookstoreApi.Repositories;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Data;
using BookstoreApi.Services;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookstoreApi.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        var database = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(database, ServerVersion.AutoDetect(database)));
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
        services.AddScoped(typeof(IBookRepository), typeof(BookRepository));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped(typeof(IPurchaseRepository), typeof(PurchaseRepository));
        services.AddScoped(typeof(IPurchaseService), typeof(PurchaseService));
        services.AddScoped(typeof(IStockService), typeof(StockService));
        services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
        services.AddScoped(typeof(IBookService), typeof(BookService));
        services.AddScoped(typeof(ITokenService), typeof(TokenService));
        services.AddScoped(typeof(IAuthService), typeof(AuthService));
        var secretKey = configuration["JWT:SecretKey"];
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                }; 
            }
            );
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireRole("Admin");
                });
            options.AddPolicy("User", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireRole("User", "Admin");
            });
        });
        
        return services;
    }
}