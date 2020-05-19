using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculasLadoDelServidor.DTOs
{
    public class RespuestaPaginada<T>
    {
        public int TotalPaginas { get; set; }
        public List<T> Registros { get; set; }
    }
}
