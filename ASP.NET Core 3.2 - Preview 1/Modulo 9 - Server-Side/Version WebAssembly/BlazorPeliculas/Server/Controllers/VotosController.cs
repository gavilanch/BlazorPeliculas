using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public VotosController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Votar(VotoPelicula votoPelicula)
        {
            var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var userId = user.Id;
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

            return NoContent();
        }
    }
}
