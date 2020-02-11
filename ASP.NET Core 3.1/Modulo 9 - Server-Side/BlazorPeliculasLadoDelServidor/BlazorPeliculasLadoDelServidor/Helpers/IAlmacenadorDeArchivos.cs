using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculasLadoDelServidor.Helpers
{
    public interface IAlmacenadorDeArchivos
    {
        Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor, string rutaArchivoActual);
        Task EliminarArchivo(string ruta, string nombreContenedor);
        Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor);
    }
}
