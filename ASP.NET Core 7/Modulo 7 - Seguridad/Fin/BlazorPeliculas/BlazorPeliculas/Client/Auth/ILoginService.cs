using BlazorPeliculas.Shared.DTOs;

namespace BlazorPeliculas.Client.Auth
{
    public interface ILoginService
    {
        Task Login(UserTokenDTO tokenDTO);
        Task Logout();
        Task ManejarRenovacionToken();
    }
}
