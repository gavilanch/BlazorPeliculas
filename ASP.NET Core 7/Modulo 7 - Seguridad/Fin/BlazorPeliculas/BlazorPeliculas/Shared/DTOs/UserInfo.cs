using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTOs
{
    public class UserInfo
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
