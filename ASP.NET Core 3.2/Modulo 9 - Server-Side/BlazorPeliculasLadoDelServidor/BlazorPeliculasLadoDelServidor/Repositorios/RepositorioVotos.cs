using BlazorPeliculasLadoDelServidor.Data;
using BlazorPeliculasLadoDelServidor.Entidades;
using BlazorPeliculasLadoDelServidor.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculasLadoDelServidor.Repositorios
{
    public class RepositorioVotos
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly AuthenticationStateService authenticationStateService;

        public RepositorioVotos(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            AuthenticationStateService authenticationStateService
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.authenticationStateService = authenticationStateService;
        }

        public async Task Votar(VotoPelicula votoPelicula)
        {
            var userId = await authenticationStateService.GetCurrentUserId();

            var votoActual = await context.VotosPeliculas
                .FirstOrDefaultAsync(x => x.PeliculaId == votoPelicula.PeliculaId && x.UserId == userId);

            if (votoActual == null)
            {
                votoPelicula.UserId = userId;
                votoPelicula.FechaVoto = DateTime.Today;
                context.Add(votoPelicula);
                await context.SaveChangesAsync();
            }
            else
            {
                votoActual.Voto = votoPelicula.Voto;
                await context.SaveChangesAsync();
            }
        }
    }
}
