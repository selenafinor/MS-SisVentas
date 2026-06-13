using Aforo255.Cross.Http.Src;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MSVenta.Venta.Repositories;
using MSVenta.Venta.Services;


namespace MSVenta.Venta
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSVenta.Venta", Version = "v1" });
            });
            services.AddDbContext<ContextDatabase>(opt =>
            {
                opt.UseMySQL(Configuration["mysql:cn"]);
            });

            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDetalleVentaService, DetalleVentaService>();

            services.AddProxyHttp();
            services.AddScoped<IUsuarioService, UsuarioService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSVenta.Venta v1"));
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}