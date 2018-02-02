using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Co_Partnership.Data;
using Co_Partnership.Models;
using Co_Partnership.Services;
using Co_Partnership.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Co_Partnership
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
            services.Configure<MvcOptions>(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 2;
                    // Signin settings
                    options.SignIn.RequireConfirmedEmail = true;
                    // User settings
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add the dbcontext here in the services in order to ba available
            services.AddDbContext<Co_PartnershipContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                //options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(15);
                //options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                //options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                //options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            // Add the item repository here
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<ITransactionRepository,TransactionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IMessageInterface, MessageInterface>();
            services.AddTransient<ICompAccountRepository, CompanyAccountRepository>();
            services.AddTransient<IWishRepository, WishRepository>();



            //services.AddTransient<IAdministratorRepository, ACAdministatorRepository>();
            services.AddTransient<IdentitySeedData, IdentitySeedData>();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();

            services.AddMemoryCache();
            services.AddSession();

            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IdentitySeedData identitySeedData)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //var options = new RewriteOptions()
            //   .AddRedirectToHttps();

            //app.UseRewriter(options);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "category",
                    template: "Products/Category/{category}/{productPage}",
                    defaults: new { Controller = "Products", Action = "Index" }
                    );
                routes.MapRoute(
                   name: "pagination",
                   template: "Products/{productPage}",
                   defaults: new { Controller = "Products", Action = "Index" }
                   );
                routes.MapRoute(
                   name: "sorting",
                   template: "Products/{productPage}",
                   defaults: new { Controller = "Products", Action = "Index" }
                   );
                routes.MapRoute(
                   name: "cart",
                   template: "Cart",
                   defaults: new { Controller = "Cart", Action = "Index" }
                   );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            identitySeedData.EnsurePopulated().Wait();
        }
    }
}
