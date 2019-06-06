using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Cadastros.Visitantes
{
    public class Visitante
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [Display(Name ="Nome")]
        public virtual string Nome { get; set; }

        [Required]       
        [Display(Name ="CPF")]
        [MaxLength(14,ErrorMessage ="O tamanho máximo desse campo é de 14 dígitos")]
        [MinLength(11,ErrorMessage ="O tamanho mínimo desse campo é de 11 dígitos")]        
        public virtual string Documento { get; set; }

        [Display(Name ="Rg")]
        public virtual string Rg { get; set; }
        
        [Phone]
        public virtual string Telefone { get; set; }

        public virtual string Empresa { get; set; }

        
        public virtual bool Bloqueado { get; set; }

        [Display(Name ="Observações")]
        public virtual string Obs { get; set; }      

        public virtual int Sequencial { get; set; }
        public virtual string FotoId { get; set; }
        public virtual string FotoPath { get; set; }
        public virtual string FotoFile { get; set; }
        public virtual string FotoBase64 { get; set; }

        
    }
}
