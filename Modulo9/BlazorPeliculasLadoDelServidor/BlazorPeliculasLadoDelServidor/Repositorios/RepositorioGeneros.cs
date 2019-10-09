using BlazorPeliculasLadoDelServidor.Data;
using BlazorPeliculasLadoDelServidor.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculasLadoDelServidor.Repositorios
{
    public class RepositorioGeneros
    {
        private readonly ApplicationDbContext context;

        public RepositorioGeneros(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Genero>> Get()
        {
            return await context.Generos.AsNoTracking().ToListAsync();
        }

        public async Task<Genero> Get(int id)
        {
            return await context.Generos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Post(Genero genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }

        public async Task Put(Genero genero)
        {
            context.Attach(genero).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);
            if (!existe) {  throw new ApplicationException($"Genero {id} no encontrado"); }
            context.Remove(new Genero { Id = id });
            await context.SaveChangesAsync();
        }

    }
}
