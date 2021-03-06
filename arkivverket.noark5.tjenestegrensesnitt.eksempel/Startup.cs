﻿using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;


namespace arkivverket.noark5.tjenestegrensesnitt.eksempel
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
            //services.AddMvc(options =>
            //    {
            //        options.RespectBrowserAcceptHeader = true;

            //        var jsonOutputFormatter = options.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
            //        jsonOutputFormatter?.SupportedMediaTypes.Clear();
            //        jsonOutputFormatter?.SupportedMediaTypes.Add("application/vnd.noark5-v4+json");
                    
            //        options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
            //        var xmlOutputFormatter = new XmlSerializerOutputFormatter();
            //        xmlOutputFormatter.SupportedMediaTypes.Clear();
            //        xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.noark5-v4+xml");
            //        options.OutputFormatters.Add(xmlOutputFormatter);
            //    })
            services.AddMvc(options =>
                {
                    options.FormatterMappings.SetMediaTypeMappingForFormat
                        ("json", MediaTypeHeaderValue.Parse("application/vnd.noark5-v4+json"));
//                    options.FormatterMappings.SetMediaTypeMappingForFormat
//                        ("xml", MediaTypeHeaderValue.Parse("application/vnd.noark5-v4+xml"));
//                    options.RespectBrowserAcceptHeader = true;
                })
                //.AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc(builder =>
            {
                builder.EnableDependencyInjection();
                builder.Expand().Select().Filter().Count().MaxTop(100).OrderBy();
            });

           }

    }
}
