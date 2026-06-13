using Aforo255.Cross.Http.Src;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MSVenta.Inventario.Repositories;
using MSVenta.Inventario.Services;

namespace MSVenta.Inventario
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSVenta.Inventario", Version = "v1" });
            });

            services.AddDbContext<ContextDatabase>(opt =>
            {
                opt.UseMySQL(Configuration["mysql:cn"]);
            });

            services.AddScoped<IArticuloService, ArticuloService>();
            services.AddScoped<IAlmacenService, AlmacenService>();
            services.AddScoped<IIngresoService, IngresoService>();
            services.AddScoped<IEgresoService, EgresoService>();
            services.AddScoped<ITraspasoService, TraspasoService>();
            services.AddScoped<IDetalleIngresoService, DetalleIngresoService>();
            services.AddScoped<IArticuloAlmacenService, ArticuloAlmacenService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IDetalleTraspasoService, DetalleTraspasoService>();
            services.AddScoped<IDetalleEgresoService, DetalleEgresoService>();
            services.AddProxyHttp();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSVenta.Inventario v1"));
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}