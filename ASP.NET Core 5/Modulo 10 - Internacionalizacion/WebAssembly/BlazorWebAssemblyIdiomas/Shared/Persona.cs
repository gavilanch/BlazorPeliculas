using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorWebAssemblyIdiomas.Shared
{
    public class Persona
    {
        [Required(
            ErrorMessageResourceType = typeof(Recursos.Resource), 
            ErrorMessageResourceName = nameof(Recursos.Resource.required))]
        public string Nombre { get; set; }
    }
}
