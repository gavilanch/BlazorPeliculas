using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public class HttpClientConToken
    {
        public HttpClientConToken(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }
    }
}
