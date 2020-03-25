using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public class CustomHttpClientFactory
    {
        private readonly HttpClient httpClient;
        private readonly IAccessTokenProvider proveedorAutenticacion;
        private readonly NavigationManager navigationManager;
        private readonly string CampoAutorizacion = "Authorization";

        public CustomHttpClientFactory(HttpClient httpClient,
            IAccessTokenProvider proveedorAutenticacion,
            NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.proveedorAutenticacion = proveedorAutenticacion;
            this.navigationManager = navigationManager;
        }

        public HttpClient ObtenerHttpClientSinToken()
        {
            if (httpClient.DefaultRequestHeaders.Contains(CampoAutorizacion))
            {
                httpClient.DefaultRequestHeaders.Remove(CampoAutorizacion);
            }

            return httpClient;
        }

        public async Task<HttpClient> ObtenerHTTPClientConToken()
        {
            if (!httpClient.DefaultRequestHeaders.Contains(CampoAutorizacion))
            {
                var resultadoToken = await proveedorAutenticacion.RequestAccessToken();

                if (resultadoToken.TryGetToken(out var token))
                {
                    httpClient.DefaultRequestHeaders.Add(CampoAutorizacion, $"Bearer {token.Value}");
                }
                else
                {
                    navigationManager.NavigateTo(resultadoToken.RedirectUrl);
                }
            }

            return httpClient;
        }
    }
}
