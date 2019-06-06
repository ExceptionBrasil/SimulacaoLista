using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Admin
{
    public class Usuario
    {
        [Key]
        public virtual int Id { get; set; }

        [Required(ErrorMessage ="O nome do usuário é obrigatório")]
        public virtual string Nome { get; set; }

        [Required(ErrorMessage ="O E-mail do usuário é obrigatório")]
        [Display(Name ="E-Mail")]
        public virtual string Email { get; set; }

        [Required]
        public virtual string Login { get; set; }

        [Required]
        [MinLength(3,ErrorMessage ="Tamanho mínimo da senha deve ser de três caracteres")]
        [Display(Name ="Senha")]
        public virtual string Password { get; set; }

        [Display(Name ="É Administrador?")]
        public  virtual bool IsAdmin { get; set; }

        public virtual DateTime? LastAcess { get; set; }

        public virtual DateTime DataCriacao { get; set; }

        public virtual Role Role { get; set; } 
        public virtual bool Ativo { get; set; }
    }
}
