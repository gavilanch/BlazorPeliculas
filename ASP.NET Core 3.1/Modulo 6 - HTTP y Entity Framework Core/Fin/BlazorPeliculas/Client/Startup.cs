using Blazor.FileReader;
using BlazorPeliculas.Client.Helpers;
using BlazorPeliculas.Client.Repositorios;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorPeliculas.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ServicioSingleton>();
            services.AddTransient<ServicioTransient>();
            services.AddScoped<IRepositorio, Repositorio>();
            services.AddScoped<IMostrarMensajes, MostrarMensajes>();
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
