using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Admin
{
    public class Login
    {
        [Required]
        [Display(Name ="Login")]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Senha")]
        public string Password { get; set; }
    }
}
