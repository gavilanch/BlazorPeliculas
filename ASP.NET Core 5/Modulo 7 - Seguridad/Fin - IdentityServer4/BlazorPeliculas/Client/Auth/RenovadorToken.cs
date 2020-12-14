using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorPeliculas.Client.Auth
{
    public class RenovadorToken : IDisposable
    {
        public RenovadorToken(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        Timer timer;
        private readonly ILoginService loginService;

        public void Iniciar()
        {
            timer = new Timer();
            timer.Interval = 5000; // 4 minutos
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            loginService.ManejarRenovacionToken();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
