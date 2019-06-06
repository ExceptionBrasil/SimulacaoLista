using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Helps
{
    public class Help
    {
        public virtual int Id {get;set;}
        public virtual Role Role { get; set; }
        public virtual string Texto { get; set; }
        public virtual DateTime? DataPublicacao { get; set; }
        public virtual DateTime DataExpiracao { get; set; }
        public virtual bool AvisoGeral { get; set; }
    }
}
