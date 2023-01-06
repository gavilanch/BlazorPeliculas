using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.Entidades
{
    public class PeliculaActor
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }
        public Actor? Actor { get; set; }
        public Pelicula? Pelicula { get; set; }
        public string? Personaje { get; set; }
        public int Orden { get; set; }
    }
}
