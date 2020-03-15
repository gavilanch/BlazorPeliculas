using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public interface IMostrarMensajes
    {
        Task MostrarMensajeError(string mensaje);
    }
}
