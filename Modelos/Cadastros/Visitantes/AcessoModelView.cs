using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Cadastros.Visitantes
{
    public class AcessoModelView
    {
        
        public virtual int Id { get; set; }

        public virtual int IdAcesso { get; set; }
        public virtual int IdVisitante { get; set; }

        [Display(Name = "Data de Entrada")]
        public virtual DateTime DataEntrada { get; set; }

        
        [Display(Name = "Data de Saída")]
        public virtual DateTime? DataSaida { get; set; }
        


        [Required]
        [Display(Name = "Nome")]
        public virtual string Nome { get; set; }

        [Required]
        [Display(Name = "CPF")]
        [MinLength(11,ErrorMessage ="Tamanho mínimo para CPF é de 11 dígitos")]        
        
        public virtual string Documento { get; set; }

        [Display(Name = "Rg")]
        public virtual string Rg { get; set; }

        
        [MaxLength(16,ErrorMessage ="Tamanho máximo para o telefone é de 16 dígitos")]
        public virtual string Telefone { get; set; }

        [Required]
        public virtual string Empresa { get; set; }

        [Required]
        [Display(Name = "Motivo da Visita")]
        [MinLength(3,ErrorMessage ="Escreva um pouco mais sobre o motiva da visita")]
        public virtual string Motivo { get; set; }

        [Display(Name = "Crachá")]        
        public virtual string IdCartao { get; set; }

        [Display(Name ="Visitado")]
        [Required]
        public virtual string Visitado { get; set; }

        
        public virtual string Foto { get; set; } //Usado para transporte apenas
        
    }
}
