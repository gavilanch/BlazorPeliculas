using Blazor.FileReader;
using BlazorPeliculas.Client.Repositorios;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddBaseAddressHttpClient();

            services.AddSingleton<ServicioSingleton>();
            services.AddTransient<ServicioTransient>();
            services.AddScoped<IRepositorio, Repositorio>();
            services.AddFileReaderService(options => options.InitializeOnFirstCall = true);
        }
    }
}
