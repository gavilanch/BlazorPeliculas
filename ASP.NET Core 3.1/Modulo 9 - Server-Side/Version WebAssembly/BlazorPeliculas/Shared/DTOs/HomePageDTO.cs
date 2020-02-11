using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorPeliculas.Shared.DTOs
{
    public class HomePageDTO
    {
        public List<Pelicula> PeliculasEnCartelera { get; set; }
        public List<Pelicula> ProximosEstrenos { get; set; }
    }
}
