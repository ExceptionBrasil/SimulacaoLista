using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Helps
{
    public class HelpView
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual int Role { get; set; }

        [Required]
        public virtual string Texto { get; set; }

        
        public virtual DateTime? DataPublicacao { get; set; }

        [Required]
        public virtual DateTime DataExpiracao { get; set; }

        [Display(Name ="Disponível a todos?")]
        public virtual bool AvisoGeral { get; set; }
    }
}
