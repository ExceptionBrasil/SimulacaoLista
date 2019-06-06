using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelos.Admin.Acessos
{
    public class MenuModelView
    {

        public int Id { get; set; }

        [Required]
        public string Controller { get; set; }

        [Required]
        public string Action { get; set; }


        public string Area { get; set; }

        //Lugar onde eu quero que apareça esse menu
        [Required]
        public string Location { get; set; }

        //Agrupador de funções
        [Required]
        public string Grupo { get; set; }

        [MinLength(6)]
        public string Descricao { get; set; }

        //Regra do Menu 
        [Required]
        public int Role { get; set; }

        [Display(Name ="Ícone")]
        public virtual string Icon { get; set; }
    }
}
