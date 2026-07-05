using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSVenta.Reportes.Services;

namespace MSVenta.Reportes
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // HttpClient para cada microservicio
            services.AddHttpClient("Ventas", client =>
            {
                client.BaseAddress = new System.Uri(
                    Configuration["MicroserviciosUrl:Ventas"]);
            });

            services.AddHttpClient("Compras", client =>
            {
                client.BaseAddress = new System.Uri(
                    Configuration["MicroserviciosUrl:Compras"]);
            });

            services.AddHttpClient("Inventario", client =>
            {
                client.BaseAddress = new System.Uri(
                    Configuration["MicroserviciosUrl:Inventario"]);
            });

            
            services.AddScoped<CorreoService>();
            services.AddScoped<VentaService>();
            services.AddScoped<CompraService>();
            services.AddScoped<InventarioService>();
            services.AddScoped<PdfService>();
            services.AddScoped<DashboardService>();

            // CORS para Angular
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });

            services.AddSwaggerGen();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                      "MSVenta.Reportes v1"));
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}