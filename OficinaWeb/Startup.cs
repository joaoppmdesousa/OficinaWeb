using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OficinaWeb.Data;
using OficinaWeb.Data.Entities;
using OficinaWeb.Helpers;
using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OficinaWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXlcdHVVQ2hYU0N/XkFWYUE=");

            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredLength = 6;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;

            })
              .AddDefaultTokenProviders()
              .AddEntityFrameworkStores<DataContext>();

            services.AddAuthentication()
                 .AddCookie()
                 .AddJwtBearer(cfg =>
                 {
                     cfg.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidIssuer = this.Configuration["Tokens:Issuer"],
                         ValidAudience = this.Configuration["Tokens:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                     };
                 });

            services.AddDbContext<DataContext>(cfg =>
                cfg.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<SeedDb>();

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            services.AddSingleton<Cloudinary>(sp =>
            {
                var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new Cloudinary(account);
            });



           

            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IConverterHelper, ConverterHelper>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<ICommunicationHelper, CommunicationHelper>();
            services.AddScoped<ICloudinaryHelper, CloudinaryHelper>();
            services.AddScoped<ICheckAppointmentHelper, CheckAppointmentHelper>();



            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IMechanicRepository, MechanicRepository>();
            services.AddScoped<IRepairAndServicesRepository, RepairAndServicesRepository>();
            services.AddScoped<IPartsRepository, PartsRepository>();
            services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
            services.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>();
            services.AddScoped<ICarBrandRepository, CarBrandRepository>();
            services.AddScoped<ICarModelRepository, CarModelRepository>();
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();



            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";

            });




            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/error/{0}");


            app.UseHttpsRedirection();
            app.UseStaticFiles();
          

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }




    }
}
