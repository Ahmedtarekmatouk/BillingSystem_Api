using Billing.Models;
using Billing.Repositories.Clients;
using Billing.Repositories.Compaines;
using Billing.Repositories.Invoices;
using Billing.Repositories.item;
using Billing.Repositories.Stock;
using Billing.Repositories.Units;
using Billing.Repositories;
using Billing.Services.Clients;
using Billing.Services.Compaines;
using Billing.Services.Invoices;
using Billing.Services.Items;
using Billing.Services.Stock;
using Billing.Services.Units;
using Billing.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Billing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            var MyAllowSpecificOrigins = "";

            #region Register Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BillingContext>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register repositories
            builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<IUnitRepository, UnitRepository>();
            builder.Services.AddScoped<IStocksRepository, StocksRepository>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            builder.Services.AddScoped<tokenRepository>(); // Add this line

            // Register services
            builder.Services.AddScoped<ICompanyService, CompanisService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IUnitService, UnitService>();
            builder.Services.AddScoped<IStockService, StockService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<IItemsService, ItemsService>();

            // Register Token service
            builder.Services.AddScoped<Tokens>();
            #endregion

            builder.Services.AddScoped<IInvoiceRepository,InvoiceRepository>();
            builder.Services.AddScoped<IInvoiceService,InvoiceService>();
           

            #region JWT Authentication Configuration
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        ValidAudience = builder.Configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //    };
            //});
            #endregion

            #region DB Configuration
            builder.Services.AddDbContext<BillingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));
            #endregion

            #region CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                           .AllowAnyHeader();
                            
                });
            });
            #endregion

            var app = builder.Build();

            #region Middleware Configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
