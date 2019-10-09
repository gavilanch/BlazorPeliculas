using BlazorPeliculasLadoDelServidor.Data;
using BlazorPeliculasLadoDelServidor.DTOs;
using BlazorPeliculasLadoDelServidor.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorPeliculasLadoDelServidor.Repositorios
{
    public class RepositorioUsuarios
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public RepositorioUsuarios(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<RespuestaPaginada<UsuarioDTO>> Get(Paginacion paginacion)
        {
            var queryable = context.Users.AsQueryable();
            var respuesta = new RespuestaPaginada<UsuarioDTO>();
            respuesta.TotalPaginas = await queryable.CalcularTotalPaginas(paginacion.CantidadRegistros);
            respuesta.Registros = await queryable.Paginar(paginacion)
                 .Select(x => new UsuarioDTO { Email = x.Email, UserId = x.Id }).ToListAsync();
            return respuesta;
        }

        public async Task<List<RolDTO>> GetRoles()
        {
            return await context.Roles
                .Select(x => new RolDTO { Nombre = x.Name, RoleId = x.Id }).ToListAsync();
        }

        public async Task AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleId));
            await userManager.AddToRoleAsync(usuario, editarRolDTO.RoleId);
        }

        public async Task RemoverUsuarioRol(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UserId);
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleId));
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RoleId);
        }
    }
}
