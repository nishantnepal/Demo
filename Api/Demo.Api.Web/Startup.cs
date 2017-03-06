using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Core.Interfaces;
using Demo.Core.Repositories;
using Demo.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.RollingFile;
using Swashbuckle.AspNetCore.Swagger;

namespace Demo.Api.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //SerilogLogger = new LoggerConfiguration().Enrich.WithExceptionDetails().WriteTo.Sink(
            //                         new RollingFileSink(@"C:\Projects\Demo\log.txt", new JsonFormatter(renderMessage: true), null, null))
            //                         .MinimumLevel.Information()
            //                         .Enrich.FromLogContext()
            //                         .CreateLogger();

        }

        public IConfigurationRoot Configuration { get; }

        // private Serilog.ILogger SerilogLogger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.GetType()
            services.AddScoped<ReservationContext>(_ => new ReservationContext(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IReservationUnitOfWork, ReservationUnitOfWork>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper();

            // services.AddSingleton<Serilog.ILogger>(SerilogLogger);

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(
                        //ignores circular references when serializing
                        opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Demo Application", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Serilog.ILogger logger = new LoggerConfiguration().Enrich.WithExceptionDetails().WriteTo
                                        .RollingFile(@"C:\Projects\Demo\log.txt")
                                     //.Sink(
                                     //new RollingFileSink(@"C:\Projects\Demo\log.txt"
                                     //, new JsonFormatter(renderMessage: true), null, null, buffered: true))
                                     .MinimumLevel.Information()
                                     .Enrich.FromLogContext()
                                     .CreateLogger();
            loggerFactory.AddSerilog(logger);

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Application V1");
            });
        }
    }
}
