using Modelos.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelos.Admin.Acessos
{
    public class Role
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual int Value { get; set; }

        [MinLength(2, ErrorMessage = "O Nome da regra deve ter pelo menos 2 caracteres")]
        [Required]
        [Display(Name ="Descrição")]
        public virtual string Description { get; set; }
    }
}
