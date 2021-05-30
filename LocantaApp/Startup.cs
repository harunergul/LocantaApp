using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocantaApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;

 
namespace LocantaApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddDbContextPool<LocantaAppDbContext>(options => {

                if (_env.IsDevelopment())
                {
                    options.UseMySQL("server=localhost;database=locantaappdb;user=appuser;password=Apppasswd123.");
                    //or
                    options.UseMySQL(Configuration.GetConnectionString("LocantaAppDb"));
                }
                else
                {
                    options.UseSqlite(Configuration.GetConnectionString("LocantaAppDb"));
                }
            
            });



            // services.AddSingleton<IRestaurantData, InMemmoryRestaturantData>();
            services.AddScoped<IRestaurantData, SqlRestaurantData>();
            services.AddControllers();
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                RequestPath = new PathString("/node_modules") 
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            app.Use(SayHelloMiddleware);
        }

        private RequestDelegate SayHelloMiddleware(RequestDelegate nextMiddleware)
        {
            return async ctx =>
            {
                if (ctx.Request.Path.StartsWithSegments("/hello"))
                {
                    await ctx.Response.WriteAsync("Hello, World!");
                }
                else
                {
                    await nextMiddleware(ctx);
                    // ctx.Response.StatusCode = 200; we can manipulate after other middlewares completed their processing
                }
            };
        }
    }
}
