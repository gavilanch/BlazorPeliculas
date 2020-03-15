using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorPeliculasLadoDelServidor.Entidades
{
    public class PeliculaActor
    {
        public int PersonaId { get; set; }
        public int PeliculaId { get; set; }
        public Persona Persona { get; set; }
        public Pelicula Pelicula { get; set; }
        public string Personaje { get; set; }
        public int Orden { get; set; }
    }
}
