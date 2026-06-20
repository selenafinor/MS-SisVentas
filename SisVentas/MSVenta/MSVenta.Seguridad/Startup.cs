using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aforo255.Cross.Token.Src;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MSVenta.Seguridad.Repositories;
using MSVenta.Seguridad.Services;

namespace MSVenta.Seguridad
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
            services.AddDbContext<ContextDatabase>(opt =>
            {
                opt.UseMySQL(Configuration["mysql:cn"]);
            });
            services.AddScoped<IPermisoService, PermisoService>();
            services.AddScoped<IRolPermisoService, RolPermisoService>();
            services.AddScoped<IRolPermisoUsuarioService, RolPermisoUsuarioService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.Configure<JwtOptions>(Configuration.GetSection("jwt"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSVenta.Seguridad", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSVenta.Seguridad v1"));
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