using AutoMapper;
using BlazorPeliculasLadoDelServidor.Data;
using BlazorPeliculasLadoDelServidor.DTOs;
using BlazorPeliculasLadoDelServidor.Entidades;
using BlazorPeliculasLadoDelServidor.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculasLadoDelServidor.Repositorios
{
    public class RepositorioPersonas
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorDeArchivos almacenadorDeArchivos;
        private readonly IMapper mapper;

        public RepositorioPersonas(ApplicationDbContext context,
            IAlmacenadorDeArchivos almacenadorDeArchivos,
            IMapper mapper)
        {
            this.context = context;
            this.almacenadorDeArchivos = almacenadorDeArchivos;
            this.mapper = mapper;
        }

        public async Task<RespuestaPaginada<Persona>> Get(Paginacion paginacion)
        {
            var queryable = context.Personas.AsQueryable();
            var respuestaPaginada = new RespuestaPaginada<Persona>();
            respuestaPaginada.TotalPaginas = await queryable.CalcularTotalPaginas(paginacion.CantidadRegistros);
            respuestaPaginada.Registros = await queryable.AsNoTracking().Paginar(paginacion).ToListAsync();
            return respuestaPaginada;
        }

        public async Task<Persona> Get(int id)
        {
            return await context.Personas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Persona>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<Persona>(); }
            textoBusqueda = textoBusqueda.ToLower();
            return await context.Personas
                .Where(x => x.Nombre.ToLower().Contains(textoBusqueda)).AsNoTracking().ToListAsync();
        }

        public async Task<int> Post(Persona persona)
        {
            if (!string.IsNullOrWhiteSpace(persona.Foto))
            {
                var fotoPersona = Convert.FromBase64String(persona.Foto);
                persona.Foto = await almacenadorDeArchivos.GuardarArchivo(fotoPersona, "jpg", "personas");
            }

            context.Add(persona);
            await context.SaveChangesAsync();
            return persona.Id;
        }

        public async Task Put(Persona persona)
        {
            var personaDB = await context.Personas.FirstOrDefaultAsync(x => x.Id == persona.Id);

            if (personaDB == null) { throw new ApplicationException($"Persona {persona.Id} no encontrada"); }

            personaDB = mapper.Map(persona, personaDB);

            if (!string.IsNullOrWhiteSpace(persona.Foto))
            {
                var fotoImagen = Convert.FromBase64String(persona.Foto);
                personaDB.Foto = await almacenadorDeArchivos.EditarArchivo(fotoImagen,
                    "jpg", "personas", personaDB.Foto);
            }

            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var existe = await context.Personas.AnyAsync(x => x.Id == id);
            if (!existe) { throw new ApplicationException($"Persona {id} no encontrada"); }
            context.Remove(new Persona { Id = id });
            await context.SaveChangesAsync();
        }
    }
}
