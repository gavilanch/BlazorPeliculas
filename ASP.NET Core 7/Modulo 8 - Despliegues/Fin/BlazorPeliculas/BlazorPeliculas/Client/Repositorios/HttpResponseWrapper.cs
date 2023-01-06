using System.Net;

namespace BlazorPeliculas.Client.Repositorios
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> ObtenerMensajeError()
        {
            if (!Error)
            {
                return null;
            }

            var codigoEstatus = HttpResponseMessage.StatusCode;

            if (codigoEstatus == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            } else if (codigoEstatus == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            } else if (codigoEstatus == HttpStatusCode.Unauthorized)
            {
                return "Tienes que loguearte para hacer esto";
            } else if (codigoEstatus == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esto";
            }
            else
            {
                return "Ha ocurrido un error inesperado";
            }
        }
    }
}
