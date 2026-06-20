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
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling =
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSVenta.Venta", Version = "v1" });
            });

            services.AddDbContext<ContextDatabase>(opt =>
            {
                opt.UseMySQL(Configuration["mysql:cn"]);
            });

            services.AddHttpClient();

            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDetalleVentaService, DetalleVentaService>();
            services.AddSingleton<RabbitMqPublisher>();
            services.AddHttpClient<LibelulaService>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.SetIsOriginAllowed(origin => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSVenta.Venta v1"));
            }

            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}