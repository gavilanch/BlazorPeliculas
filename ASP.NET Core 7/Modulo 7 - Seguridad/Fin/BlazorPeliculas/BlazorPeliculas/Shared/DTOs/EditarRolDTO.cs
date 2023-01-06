using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTOs
{
    public class EditarRolDTO
    {
        public string UsuarioId { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
