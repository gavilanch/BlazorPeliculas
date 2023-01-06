using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTOs
{
    public class PeliculaVisualizarDTO
    {
        public Pelicula Pelicula { get; set; } = null!;
        public List<Genero> Generos { get; set; } = new List<Genero>();
        public List<Actor> Actores { get; set; } = new List<Actor>();
        public int VotoUsuario { get; set; }
        public double PromedioVotos { get; set; }
    }
}
