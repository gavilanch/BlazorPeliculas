using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
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
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorDeArchivos almacenadorDeArchivos;
        private readonly IMapper mapper;

        public PeliculasController(ApplicationDbContext context, 
            IAlmacenadorDeArchivos almacenadorDeArchivos,
            IMapper mapper)
        {
            this.context = context;
            this.almacenadorDeArchivos = almacenadorDeArchivos;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limite = 6;

            var peliculasEnCartelera = await context.Peliculas
                .Where(x => x.EnCartelera).Take(limite)
                .OrderByDescending(x => x.Lanzamiento)
                .ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await context.Peliculas
                .Where(x => x.Lanzamiento > fechaActual)
                .OrderBy(x => x.Lanzamiento).Take(limite)
                .ToListAsync();

            var response = new HomePageDTO()
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                ProximosEstrenos = proximosEstrenos
            };

            return response;

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaVisualizarDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas.Where(x => x.Id == id)
                .Include(x => x.GenerosPelicula).ThenInclude(x => x.Genero)
                .Include(x => x.PeliculasActor).ThenInclude(x => x.Persona)
                .FirstOrDefaultAsync();

            if (pelicula == null) { return NotFound(); }

            // todo: sistema de votacion
            var promedioVotos = 4;
            var votoUsuario = 5;

            pelicula.PeliculasActor = pelicula.PeliculasActor.OrderBy(x => x.Orden).ToList();

            var model = new PeliculaVisualizarDTO();
            model.Pelicula = pelicula;
            model.Generos = pelicula.GenerosPelicula.Select(x => x.Genero).ToList();
            model.Actores = pelicula.PeliculasActor.Select(x =>
            new Persona
            {
                Nombre = x.Persona.Nombre,
                Foto = x.Persona.Foto,
                Personaje = x.Personaje,
                Id = x.PersonaId
            }).ToList();

            model.PromedioVotos = promedioVotos;
            model.VotoUsuario = votoUsuario;
            return model;
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<Pelicula>>> Get([FromQuery] ParametrosBusquedaPeliculas parametrosBusqueda)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parametrosBusqueda.Titulo))
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.Titulo.ToLower().Contains(parametrosBusqueda.Titulo.ToLower()));
            }

            if (parametrosBusqueda.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCartelera);
            }

            if (parametrosBusqueda.Estrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.Lanzamiento >= hoy);
            }

            if (parametrosBusqueda.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.GenerosPelicula.Select(y => y.GeneroId)
                    .Contains(parametrosBusqueda.GeneroId));
            }

            // TODO: Implementar votacion

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(peliculasQueryable, 
                parametrosBusqueda.CantidadRegistros);

            var peliculas = await peliculasQueryable.Paginar(parametrosBusqueda.Paginacion).ToListAsync();

            return peliculas;
        }

        public class ParametrosBusquedaPeliculas
        {
            public int Pagina { get; set; } = 1;
            public int CantidadRegistros { get; set; } = 10;
            public Paginacion Paginacion
            {
                get { return new Paginacion() { Pagina = Pagina, CantidadRegistros = CantidadRegistros }; }
            }
            public string Titulo { get; set; }
            public int GeneroId { get; set; }
            public bool EnCartelera { get; set; }
            public bool Estrenos { get; set; }
            public bool MasVotadas { get; set; }
        }

        [HttpGet("actualizar/{id}")]
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult) { return NotFound(); }

            var peliculaVisualizarDTO = peliculaActionResult.Value;
            var generosSeleccionadosIds = peliculaVisualizarDTO.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSeleccionadosIds.Contains(x.Id))
                .ToListAsync();

            var model = new PeliculaActualizacionDTO();
            model.Pelicula = peliculaVisualizarDTO.Pelicula;
            model.GenerosNoSeleccionados = generosNoSeleccionados;
            model.GenerosSeleccionados = peliculaVisualizarDTO.Generos;
            model.Actores = peliculaVisualizarDTO.Actores;
            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var fotoPersona = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await almacenadorDeArchivos.GuardarArchivo(fotoPersona, "jpg", "peliculas");
            }

            if (pelicula.PeliculasActor != null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Pelicula pelicula)
        {
            var peliculaDB = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == pelicula.Id);

            if (peliculaDB == null) { return NotFound(); }

            peliculaDB = mapper.Map(pelicula, peliculaDB);

            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var posterImagen = Convert.FromBase64String(pelicula.Poster);
                peliculaDB.Poster = await almacenadorDeArchivos.EditarArchivo(posterImagen,
                    "jpg", "peliculas", peliculaDB.Poster);
            }

            await context.Database.ExecuteSqlInterpolatedAsync($"delete from GenerosPeliculas WHERE PeliculaId = {pelicula.Id}; delete from PeliculasActores where PeliculaId = {pelicula.Id}");

            if (pelicula.PeliculasActor != null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }

            peliculaDB.PeliculasActor = pelicula.PeliculasActor;
            peliculaDB.GenerosPelicula = pelicula.GenerosPelicula;

            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Peliculas.AnyAsync(x => x.Id == id);
            if (!existe) { return NotFound(); }
            context.Remove(new Pelicula { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
