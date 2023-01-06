using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorPeliculas.Client.Auth
{
    public class ProveedorAutenticacionPrueba : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimo = new ClaimsIdentity();
            var usuarioFelipe = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim("llave1", "valor1"),
                    new Claim("edad", "999"),
                    new Claim(ClaimTypes.Name, "Felipe")
                    //new Claim(ClaimTypes.Role, "admin")
                },
                authenticationType: "prueba");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));
        }
    }
}
