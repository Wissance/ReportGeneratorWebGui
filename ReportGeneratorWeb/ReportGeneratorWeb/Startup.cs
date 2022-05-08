using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;


namespace ReportGeneratorWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                                      .AddJsonFile($"appsettings.{environment.EnvironmentName}.json")
                                                      .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 1. MVC
            services.AddMvc();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
            });
            // 2. Logging
            services.AddLogging(loggingBuilder => loggingBuilder.AddConfiguration(Configuration).AddConsole());
            services.AddLogging(loggingBuilder => loggingBuilder.AddConfiguration(Configuration).AddDebug());
            services.AddLogging(loggingBuilder => loggingBuilder.AddConfiguration(Configuration).AddSerilog(dispose: true));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseDefaultFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink(); // https://kmatyaszek.github.io/2020/02/29/how-to-enable-browser-link-in-aspnetcore3-app.html
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            RewriteOptions options = new RewriteOptions().AddRedirect("(.*[^/])$", "$1/");
            //.AddRedirectToHttpsPermanent();
            app.UseRewriter(options);
            app.UseRouting();
            app.UseStaticFiles();
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });

            app.UseMvc();
        }

        public IConfiguration Configuration { get; }
    }
}
