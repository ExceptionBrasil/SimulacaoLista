using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelos.Admin.Acessos
{
    public class Menu
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string Controller { get; set; }

        [Required]
        public virtual string Action { get; set; }


        public virtual string Area { get; set; }
        
        //Lugar onde eu quero que apareça esse menu
        [Required]
        public virtual string Location { get; set; }

       //Agrupador de funções
        [Required]
        public virtual string Grupo { get; set; }

        [MinLength(6)]
        [Display(Name ="Descrição")]
        public virtual string Descricao { get; set; }      
        
        //Regra do Menu 
        [Required]
        public virtual Role Role { get; set; }

        public virtual string Icon { get; set; }
    }
}
