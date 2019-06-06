using Modelos.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Cadastros.Visitantes
{
    public class ControleDeAcesso 
    {
        [Key]
        public virtual int Id { get; set; }

      
        [Display(Name ="Data de Entrada")]
        public virtual DateTime DataEntrada { get; set; }

        
        [Display(Name ="Data de Saída")]
        public virtual DateTime? DataSaida { get; set; }

        public virtual Visitante Visitante { get; set; }

        [Required]
        [Display(Name = "Motivo da Visita")]
        public virtual string Motivo { get; set; }

        [Display(Name ="Crachá")]       
        public virtual string IdCartao { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual DateTime DataInclusao { get; set; }

        public virtual string Visitado { get; set; }        

    }
}
