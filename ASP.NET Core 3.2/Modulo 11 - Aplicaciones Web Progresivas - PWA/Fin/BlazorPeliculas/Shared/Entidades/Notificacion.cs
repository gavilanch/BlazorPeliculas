using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorPeliculas.Shared.Entidades
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
