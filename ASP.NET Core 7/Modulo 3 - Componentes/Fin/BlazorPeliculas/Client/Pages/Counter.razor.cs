using BlazorPeliculas.Client.Helpers;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject] ServicioSingleton singleton { get; set; } = null!;
        [Inject] ServicioTransient transient { get; set; } = null!;
        [Inject] IJSRuntime js { get; set; } = null!;
        //[CascadingParameter(Name = "Color")] protected string Color { get; set; } = null!;
        //[CascadingParameter(Name = "Size")] protected string Size { get; set; } = null!;
        [CascadingParameter] protected AppState appState { get; set; } = null!;

        IJSObjectReference? modulo;

        private int currentCount = 0;
        private static int currentCountStatic = 0;

        [JSInvokable]
        public async Task IncrementCount()
        {
            modulo = await js.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");
            await modulo.InvokeVoidAsync("mostrarAlerta", "hola mundo");

            currentCount += 1;
            currentCountStatic = currentCount;
            singleton.Valor = currentCount;
            transient.Valor = currentCount;
            await js.InvokeVoidAsync("pruebaPuntoNetStatic");
        }

        private async Task IncrementCountJavaScript()
        {
            await js.InvokeVoidAsync("pruebaPuntoNetInstancia",
                DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public static Task<int> ObtenerCurrentCount()
        {
            return Task.FromResult(currentCountStatic);
        }
    }
}
