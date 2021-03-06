﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfoAPI.Entities;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace CityInfoAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        public static IConfiguration configuration { get; private set; }

        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                //.AddJsonOptions(op =>
                //{ //Regresa las prop de json en Mayusculas, la primer letra
                //    var castedResolver = op.SerializerSettings.ContractResolver
                //                          as DefaultContractResolver;
                //    castedResolver.NamingStrategy = null;
                //})
                .AddMvcOptions(op => op.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()
                    ))
                ;
            

            //AddTransiet  - AddScope - AddSingleton 3 formas de inyectar servicios Duracion de service

            //AddTransiet cada vez que es solicita y son servicios ligeros y no compraten info 
            //AddScope se crea una vez por solicitud
            //AddSingleton son unicos una sola vez (al hacer un carrito de compras)
            //Crear el servicio, agregar el servicio y usarlo en la clase que quieras en el constructor

            //Directivas para ver cuando usar cierto servicio
            /*
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
             services.AddTransient<IMailService, CloudMailService>();
#endif
*/

            var connectionString = Startup.configuration["connectionString:cityInfoDb"];
            services.AddDbContext<CityInfoDbContext>(o => o.UseSqlServer(connectionString));

            //Inyectar el repositorio creado CityInfoRepository inyectarlo en el controlador
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoDbContext context)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            context.EnsureSeedDatForContext();
            app.UseStatusCodePages();

            //Auto Mapper Configuracion
            AutoMapper.Mapper.Initialize(cfg=> 
            {
                //Mapear Entidad ciudad a Dto Ciudad 
                cfg.CreateMap<Entities.City, Models.CityWithoutPointOfInterestDto>();
                cfg.CreateMap<Entities.City, Models.CiudadDto>();
                cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
                //Crear Punto de Interes
                cfg.CreateMap<Models.PointOfInteresForCreation, Entities.PointOfInterest>();
            });

            app.UseMvc();
           
            //app.Run(async (context) =>
            //{
                
            //   // await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
