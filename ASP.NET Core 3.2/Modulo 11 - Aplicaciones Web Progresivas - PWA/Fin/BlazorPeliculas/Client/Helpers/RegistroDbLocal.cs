using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public class RegistroDbLocal
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public JsonElement Cuerpo { get; set; }
    }

    public class RegistrosDbLocal
    {
        public List<RegistroDbLocal> ObjetosACrear { get; set; } = new List<RegistroDbLocal>();
        public List<RegistroDbLocal> ObjetosABorrar { get; set; } = new List<RegistroDbLocal>();

        public int ObtenerPendientes()
        {
            var resultado = 0;

            resultado += ObjetosACrear.Count;
            resultado += ObjetosABorrar.Count;

            return resultado;
        }

    }
}
