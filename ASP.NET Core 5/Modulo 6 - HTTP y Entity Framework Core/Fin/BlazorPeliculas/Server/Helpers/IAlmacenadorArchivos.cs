using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Helpers
{
    public interface IAlmacenadorArchivos
    {
        Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor);
        Task EliminarArchivo(string ruta, string nombreContenedor);
        Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor, string ruta);
    }
}
