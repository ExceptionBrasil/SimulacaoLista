using FluentNHibernate.Mapping;
using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Mappings.Admin
{
    public class MenuMapping : ClassMap<Menu>
    {
        public MenuMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Area);
            Map(x => x.Controller);
            Map(x => x.Action);
            Map(x => x.Descricao);            
            Map(x => x.Location);
            Map(m => m.Grupo);
            Map(m => m.Icon);
            References(x => x.Role);
        }
    }
}
