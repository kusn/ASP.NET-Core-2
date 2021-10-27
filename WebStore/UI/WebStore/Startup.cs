﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Services.Services.InCookies;
using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Values;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Identity;
using Microsoft.Extensions.Logging;
using WebStore.Logger;
using WebStore.Services.Services;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddIdentityWebStoreWebAPIClients()
                .AddDefaultTokenProviders();

            //services.AddIdentityWebStoreWebAPIClients();
            //services.AddHttpClient("WebStoreWebAPIIdentity", client => client.BaseAddress = new(Configuration["WebAPI"]))
            //    .AddTypedClient<IUserStore<User>, UsersClient>()
            //    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
            //    .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
            //    .AddTypedClient<IUserEmailStore<User>, UsersClient>()
            //    .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
            //    .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
            //    .AddTypedClient<IUserClaimStore<User>, UsersClient>()
            //    .AddTypedClient<IUserLoginStore<User>, UsersClient>()
            //    .AddTypedClient<IRoleStore<Role>, RolesClient>()
            //    ;

            services.Configure<IdentityOptions>(opt => 
            {
#if DEBUG
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";
                
                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(15);

            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "GB.WebStore";
                opt.Cookie.HttpOnly = true;

                opt.ExpireTimeSpan = System.TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccsessDenied";

                opt.SlidingExpiration = true;
            });
            
            //services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<ICartStore, InCookiesCartStore>();
            services.AddScoped<ICartService, CartService>();

            services.AddHttpClient("WebStoreWebAPI", client =>
            client.BaseAddress = new(Configuration["WebAPI"]))
                .AddTypedClient<IValuesService, ValuesClient>()
                .AddTypedClient<IEmployeesData, EmployeesClient>()
                .AddTypedClient<IProductData, ProductsClient>()
                .AddTypedClient<IOrderService, OrdersClient>();

            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention()))
                .AddRazorRuntimeCompilation();
        }
                
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Status/", "?code={0}");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<TestMiddleware>();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            //var loggin = Configuration["Loggin:LogLevel:Default"];
            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapGet("/greetings", async context =>
                {
                    var greetings = Configuration["Greetings"];
                    await context.Response.WriteAsync(greetings);
                });

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
