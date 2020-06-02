using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerIdiomas.Controllers
{
    [Route("[controller]/[action]")]
    public class CulturaController : Controller
    {
        public IActionResult SetearCultura(string cultura, string redireccionURL)
        {
            if (cultura != null)
            {
                HttpContext.Response.Cookies.Append(
                     CookieRequestCultureProvider.DefaultCookieName,
                     CookieRequestCultureProvider.MakeCookieValue(
                         new RequestCulture(cultura)));
            }

            return LocalRedirect(redireccionURL);
        }
    }
}
