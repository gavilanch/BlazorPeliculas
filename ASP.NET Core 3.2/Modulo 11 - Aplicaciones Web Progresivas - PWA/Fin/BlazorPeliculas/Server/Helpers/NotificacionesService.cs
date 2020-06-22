using BlazorPeliculas.Shared.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebPush;

namespace BlazorPeliculas.Server.Helpers
{
    public class NotificacionesService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public NotificacionesService(IConfiguration configuration, ApplicationDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        public async Task EnviarNotificacionPeliculaEnCartelera(Pelicula pelicula)
        {
            var notificaciones = await context.Notificaciones.ToListAsync();

            var llavePublica = configuration.GetValue<string>("notificaciones:llave_publica");
            var llavePrivada = configuration.GetValue<string>("notificaciones:llave_privada");
            var email = configuration.GetValue<string>("notificaciones:email");

            var vapidDetails = new VapidDetails(email, llavePublica, llavePrivada);

            foreach (var notificacion in notificaciones)
            {
                var pushSubscription = new PushSubscription(notificacion.URL,
                    notificacion.P256dh, notificacion.Auth);

                var webPushClient = new WebPushClient();

                try
                {
                    var payload = JsonSerializer.Serialize(new
                    {
                        titulo = pelicula.Titulo,
                        imagen = pelicula.Poster,
                        url = $"pelicula/{pelicula.Id}"
                    });

                    await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // ...
                }
            }

        }
    }
}
