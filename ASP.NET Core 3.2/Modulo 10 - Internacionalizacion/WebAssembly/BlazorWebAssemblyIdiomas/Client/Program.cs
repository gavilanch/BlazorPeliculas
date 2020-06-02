using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Globalization;

namespace BlazorWebAssemblyIdiomas.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddLocalization();

            var host = builder.Build();
            var js = host.Services.GetRequiredService<IJSRuntime>();
            var cultura = await js.InvokeAsync<string>("cultura.get");

            if (cultura == null)
            {
                var culturaPorDefecto = new CultureInfo("en-US");
                CultureInfo.DefaultThreadCurrentCulture = culturaPorDefecto;
                CultureInfo.DefaultThreadCurrentUICulture = culturaPorDefecto;
            }
            else
            {
                var culturaUsuario = new CultureInfo(cultura);
                CultureInfo.DefaultThreadCurrentCulture = culturaUsuario;
                CultureInfo.DefaultThreadCurrentUICulture = culturaUsuario;
            }

            await builder.Build().RunAsync();
        }
    }
}
