using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Mensageria
{
    public class MensagensTruck
    {
        public virtual int Id { get; set; }

        [Required]
        [Display(Name ="Mensagem")]
        public virtual string Mensagem { get; set; }

        
        public virtual DateTime DataCriacao { get; set; }
    }
}
