using BlazorPeliculas.Shared.Entidades;
using System.Text;
using System.Text.Json;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositorio
    {
        private readonly HttpClient httpClient;

        public Repositorio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private JsonSerializerOptions OpcionesPorDefectoJSON => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var respuestaHTTP = await httpClient.GetAsync(url);

            if (respuestaHTTP.IsSuccessStatusCode)
            {
                var respuesta = await DeserializarRespuesta<T>(respuestaHTTP, OpcionesPorDefectoJSON);
                return new HttpResponseWrapper<T>(respuesta, error: false, respuestaHTTP);
            }
            else
            {
                return new HttpResponseWrapper<T>(default, error: true, respuestaHTTP);
            }
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PutAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var responseHTTP = await httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, 
                !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<TResponse>(responseHttp, 
                    OpcionesPorDefectoJSON);
                return new HttpResponseWrapper<TResponse>(response, error: false, responseHttp);
            }

            return new HttpResponseWrapper<TResponse>(default, 
                !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> DeserializarRespuesta<T>(HttpResponseMessage httpResponse, 
            JsonSerializerOptions jsonSerializerOptions)
        {
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions);
        }

        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
            {
                new Pelicula{Titulo = "Wakanda Forever",
                    Lanzamiento = new DateTime(2022, 11, 11),
                    Poster = "https://upload.wikimedia.org/wikipedia/en/3/3b/Black_Panther_Wakanda_Forever_poster.jpg"
                },
                new Pelicula{Titulo = "Moana",
                    Lanzamiento = new DateTime(2016, 11, 23),
                Poster = "https://upload.wikimedia.org/wikipedia/en/2/26/Moana_Teaser_Poster.jpg"
                },
                new Pelicula{Titulo = "Inception",
                    Lanzamiento = new DateTime(2010, 7, 16),
                Poster = "https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg"}
            };
        }
    }
}
