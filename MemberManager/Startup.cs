using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberManager.Context;
using MemberManager.Filters;
//using MemberManager.Filters;
using MemberManager.Manager;
using MemberManager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MemberManager
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
            services.AddDbContext<MemberContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MemberConnString"),
            sqlServerOptions => sqlServerOptions.CommandTimeout(300)));
            //services.AddControllersWithViews();

            services.AddScoped<MembersManager>();
            services.AddScoped<OrderDetailsManager>();
            services.AddScoped<OrderDetailStatusManager>();
            services.AddScoped<OrdersManager>();
            services.AddScoped<OrderStatusManager>();
            services.AddScoped<ProductsManager>();
            services.AddScoped<ProductTypesManager>();
            services.AddScoped<SendTypesManager>();
            services.AddScoped<SysRolesManager>();
            services.AddScoped<MemberRolesManager>();
            services.AddScoped<SysFunctionsManager>();
            services.AddScoped<SysRolesFunctionsManager>();

            services.AddScoped<ProductsService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // 將 Session 存在 ASP.NET Core 記憶體中
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options =>
            {
                options.Filters.Add<UserLoginFilter>();

            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSignalR();
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //不加上這句專案內中使用session會報錯
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
