using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
{
                    new Pelicula(){Titulo = "Spider-Man: Far From Home", Lanzamiento  = new DateTime(2019, 7, 2)},
                    new Pelicula(){Titulo = "Moana", Lanzamiento  = new DateTime(2016, 11, 23)},
                    new Pelicula(){Titulo = "Inception", Lanzamiento  = new DateTime(2010, 7, 16)}
                };
        }
    }
}
