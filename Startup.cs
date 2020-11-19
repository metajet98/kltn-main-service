using System.Text;
using main_service.Databases;
using main_service.Repositories;
using main_service.Storage;
using main_service.Utils.EncryptionHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace main_service
{
    public class Startup
    {
        readonly string myAllowAllOrigins = "_myAllowAllOrigins";

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: myAllowAllOrigins,
                    builder => { 
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod(); 
                    });
            });
            
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MaintenanceSystem"));
            });
            
            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Auth:JwtSecretCode").Value);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddAuthorization();
            services.AddSingleton<IEncryptionHelper, EncryptionHelper>();
            services.AddSingleton<StorageManager>();
            
            services.AddScoped<UserRepository>();
            services.AddScoped<CompanyRepository>();
            services.AddScoped<VehicleTypeRepository>();
            services.AddScoped<VehicleGroupRepository>();
            services.AddScoped<UserAuthRepository>();
            services.AddScoped<UserVehicleRepository>();
            services.AddScoped<BranchRepository>();
            services.AddScoped<SparePartRepository>();
            services.AddScoped<SparePartCheckingStatusRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(myAllowAllOrigins);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

           app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}