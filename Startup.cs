using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Catalog.Repositories;
using MongoDB.Driver;
using Catalog.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace catalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) //register all service im using acrross application
        {


//anytime it sees a guid,it should,serialise them as string in the databse
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();  

//this connects to mongodb repository
            services.AddSingleton<IMongoClient>(serviceProvider => 
            {
                var settings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                return new MongoClient(mongoDbSettings.ConnectionString);
            });

             //registers our dependency   
            services.AddSingleton<IItemsRespository, MongoDbItemsRepository>();  //havig one instance of a type accross entire instance of our service
            services.AddControllers(options => {
                options.SuppressAsyncSuffixInActionNames =false; // dotnet will not remove the async suffixs at runtime
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "catalog", Version = "v1" });
            });

            //added for endpoints below
            //added nuget package dotnet add package AspNetCore.HealthChecks.MongoDb
            services.AddHealthChecks()
                .AddMongoDb(mongoDbSettings.ConnectionString, 
                name:"mongodb", 
                timeout: TimeSpan.FromSeconds(3), //timesout after 3 seconds
                tags: new[]{"ready"} //group everything single health chek
                );
                //this takes mongodbsettings string from above and chekcs connection and timesout afte three seconds
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //request pipline for middlewears
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "catalog v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions{
                    Predicate = (check) => check.Tags.Contains("ready"),   //will only include health checks that have ready
                    ResponseWriter = async(context, report)=>{
                        var result= JsonSerializer.Serialize(
                            new{
                                status =report.Status.ToString(),
                                checks = report.Entries.Select(entry => new{  //new annonynmous type
                                    name =entry.Key,
                                    status = entry.Value.Status.ToString(),
                                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message: "none", //if there is an excpetion message pritn it, if not then it's equal to 'none'...not sure if there is one
                                    duration = entry.Value.Duration.ToString()  //how long health check took 
                                })
                            }
                        );
                    context.Response.ContentType= MediaTypeNames.Application.Json; //formats output frmo aobove
                    await context.Response.WriteAsync(result); //writes this information out

                    }   //spcifigy how to render the results of health check 
                }); //a healthcheck endpoint-need above staement in configservices

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions{
                    Predicate = (check) => false    //it'll come back to us along as the restapi is alive,excludes mongodb
            });
            
        });
    }
    }
}
