using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MSVenta.Compras.Repositories;
using MSVenta.Compras.Services;

namespace MSVenta.Compras
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSVenta.Compras", Version = "v1" });
            });

            services.AddDbContext<ContextDatabase>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpClient();
            services.AddScoped<IProveedorService, ProveedorService>();
            services.AddScoped<INotaCompraService, NotaCompraService>();
            services.AddSingleton<RabbitMqPublisher>();
            services.AddScoped<ICatalogoProveedorService, CatalogoProveedorService>();

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSVenta.Compras v1"));
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