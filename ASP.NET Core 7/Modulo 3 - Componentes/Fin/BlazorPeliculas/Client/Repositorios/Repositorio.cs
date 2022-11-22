using BlazorPeliculas.Shared.Entidades;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
            {
                new Pelicula{Titulo = "Wakanda Forever", FechaLanzamiento = new DateTime(2022, 11, 11)},
                new Pelicula{Titulo = "Moana", FechaLanzamiento = new DateTime(2016, 11, 23)},
                new Pelicula{Titulo = "Inception", FechaLanzamiento = new DateTime(2010, 7, 16)}
            };
        }
    }
}
