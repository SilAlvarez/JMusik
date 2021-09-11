using AutoMapper;
using JMusik.Data;
using JMusik.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;


namespace JMusik.WebApi
{
    public class Startup
    {
        //private const string Key = "SecretKey";

        public Startup(IConfiguration configuration)
        {

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers(config => {
                config.ReturnHttpNotAcceptable = true;
            }
            
           )            
                .AddXmlDataContractSerializerFormatters();

            services.AddDbContext<TiendaDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TiendaDb")));


            services.ConfigureDependencies();
            services.ConfigureJwt(Configuration);
            services.ConfigureCors();


        }

   

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerfactory)
        {
            loggerfactory.AddSerilog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
