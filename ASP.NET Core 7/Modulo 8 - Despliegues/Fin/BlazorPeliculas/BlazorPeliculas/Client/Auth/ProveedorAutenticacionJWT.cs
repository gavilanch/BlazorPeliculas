using BlazorPeliculas.Client.Helpers;
using BlazorPeliculas.Client.Repositorios;
using BlazorPeliculas.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorPeliculas.Client.Auth
{
    public class ProveedorAutenticacionJWT : AuthenticationStateProvider, ILoginService
    {
        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;
        private readonly IRepositorio repositorio;

        public ProveedorAutenticacionJWT(IJSRuntime js, HttpClient httpClient,
            IRepositorio repositorio)
        {
            this.js = js;
            this.httpClient = httpClient;
            this.repositorio = repositorio;
        }

        public static readonly string TOKENKEY = "TOKENKEY";
        public static readonly string EXPIRATIONTOKENKEY = "EXPIRATIONTOKENKEY";

        private AuthenticationState Anonimo =>
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.ObtenerDeLocalStorage(TOKENKEY);

            if (token is null)
            {
                return Anonimo;
            }

            var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(EXPIRATIONTOKENKEY);
            DateTime tiempoExpiracion;

            if (tiempoExpiracionObject is null)
            {
                await Limpiar();
                return Anonimo;
            }

            if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
            {
                if (TokenExpirado(tiempoExpiracion))
                {
                    await Limpiar();
                    return Anonimo;
                }

                if (DebeRenovarToken(tiempoExpiracion))
                {
                    token = await RenovarToken(token.ToString()!);
                }
            }

            return ConstruirAuthenticationState(token.ToString()!);
        }

        private bool TokenExpirado(DateTime tiempoExpiracion)
        {
            return tiempoExpiracion <= DateTime.UtcNow;
        }

        private bool DebeRenovarToken(DateTime tiempoExpiracion)
        {
            return tiempoExpiracion.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(5);
        }

        public async Task ManejarRenovacionToken()
        {
            var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(EXPIRATIONTOKENKEY);
            DateTime tiempoExpiracion;

            if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion))
            {
                if (TokenExpirado(tiempoExpiracion))
                {
                    await Logout();
                }

                if (DebeRenovarToken(tiempoExpiracion))
                {
                    var token = await js.ObtenerDeLocalStorage(TOKENKEY);
                    var nuevoToken = await RenovarToken(token.ToString()!);
                    var authState = ConstruirAuthenticationState(nuevoToken);
                    NotifyAuthenticationStateChanged(Task.FromResult(authState));
                }
            }
        }

        private async Task<string> RenovarToken(string token)
        {
            Console.WriteLine("Renovando el token...");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);

            var nuevoTokenResponse = await repositorio.Get<UserTokenDTO>("api/cuentas/RenovarToken");
            var nuevoToken = nuevoTokenResponse.Response!;

            await js.GuardarEnLocalStorage(TOKENKEY, nuevoToken.Token);
            await js.GuardarEnLocalStorage(EXPIRATIONTOKENKEY, nuevoToken.Expiration.ToString());

            return nuevoToken.Token;
        }

        private AuthenticationState ConstruirAuthenticationState(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", token);
            var claims = ParsearClaimsDelJWT(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        private IEnumerable<Claim> ParsearClaimsDelJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenDeserializado = jwtSecurityTokenHandler.ReadJwtToken(token);
            return tokenDeserializado.Claims;
        }

        public async Task Login(UserTokenDTO tokenDTO)
        {
            await js.GuardarEnLocalStorage(TOKENKEY, tokenDTO.Token);
            await js.GuardarEnLocalStorage(EXPIRATIONTOKENKEY, tokenDTO.Expiration.ToString());
            var authState = ConstruirAuthenticationState(tokenDTO.Token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await Limpiar();
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }

        private async Task Limpiar()
        {
            await js.RemoverDelLocalStorage(TOKENKEY);
            await js.RemoverDelLocalStorage(EXPIRATIONTOKENKEY);
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
