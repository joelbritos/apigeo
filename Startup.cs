using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGEO.Business;
using APIGEO.Business.Contracts;
using APIGEO.Repository;
using APIGEO.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace APIGEO
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
            var server = Configuration["DBServer"] ?? "ms-sql-server";
            var port = Configuration["Port"] ?? "1433";
            var user = Configuration["DBUser"] ?? "SA";
            var password = Configuration["DBPassword"] ?? "Developer2021";
            var database = Configuration["Database"] ?? "APIGEODB";
            services.AddDbContext<APIGEOContext>(opt => 
            opt.UseSqlServer($"Server={server},{port};Initial Catalog={database};User Id={user};Password={password};")
            );

            //Configuracion el productor de mensajes
            services.AddSingleton<ProducerConfig>(opt => {
                ProducerConfig config = new ProducerConfig();
                config.BootstrapServers = Configuration["BootstrapServers"] ?? "";
                return config;
            });

            //Configuracion del consumidor de mensajes
            services.AddHostedService<ConsumerService>();
            services.AddSingleton<ConsumerConfig>(opt => 
            {
                ConsumerConfig config = new ConsumerConfig();
                config.BootstrapServers = Configuration["BootstrapServers"] ?? "";
                config.GroupId = Guid.NewGuid().ToString();
                config.AutoOffsetReset = AutoOffsetReset.Earliest;

                return config;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IGeolocalizarBusiness, GeolocalizarBusiness>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDB.PrepPopulation(app);
        }
    }
}
