using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SourceCode.Custom;
using SourceCode.Models;
using SourceCode.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;

namespace SourceCode
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // We Created AppDBContext class inheriting IdentityDBContext class to use Microsoft Identity features in our application.
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer("Data Source=DESKTOP-I6I0EIC;Initial Catalog=AdBill;Integrated Security = True;"));
            //Also need to Plug In Identity Service To use Microsoft Identity
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDBContext>();

            // Change Password Complexity For the User
            services.Configure<IdentityOptions>(options =>
            {
                // We set complexity to very easy, you can change as you want.
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            //To Change Default Login Path if user is not Authenticated
            services.ConfigureApplicationCookie(options => options.LoginPath = "/WebMaster/LoginPanel");

            //Below code is used to use US culture for dates, timezones, etc. You can set any valid culture you want.
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
            });
           
            // For using MVC we plug in below service.
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;
            // AddJsonOptions is used to keep Property Names As UpperCamelCase 
            //else it will lower the first character of Property Name in models

            // Dependency Services we created & used are given below : 
            services.AddScoped<IGeneralService, GeneralService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IEmailService, EmailService>();

            // For API
            services.AddScoped<ICommonApiService, CommonApiService>();
            services.AddApplicationInsightsTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            UserManager<IdentityUser> userManager, IConfiguration _config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use this for localization for US as described in services section above.
            app.UseRequestLocalization();

            // For requesting static files
            app.UseStaticFiles();

            // For using identity authentication
            app.UseAuthentication();

            //Below class is used to seed Admin User in to Database
            SeedUsers.AddUsers(userManager, _config);

            // MVC Usage with inital route defined
            app.UseMvc(options =>
            {
                options.MapRoute(
                name: "Default",
                template: "{Controller=Webmaster}/{Action=LoginPanel}/{id?}"
                );

                options.MapRoute(
                name: "api",
                template: "api/{Controller}/{Action}/{id?}"
                );
            });
           
            // If Request is not handled by MVC, this last context is used in Request Pipeline , uncomment this if you want.
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Nothing Found, Please Contact Admin.");
            });
        }
    }
}
