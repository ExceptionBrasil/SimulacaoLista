using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Helps
{
    public class HelpMapping:ClassMap<Help>
    {
        public HelpMapping()
        {
            Id(h => h.Id);
            Map(h => h.Texto);
            Map(h => h.DataPublicacao);
            Map(h => h.DataExpiracao);
            Map(h => h.AvisoGeral);
            References(h => h.Role);
        }
    }
}
