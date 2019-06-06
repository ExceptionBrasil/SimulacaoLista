using FluentNHibernate.Mapping;
using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Mappings.Admin
{
    public class RoleMapping:ClassMap<Role>
    {
        public RoleMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description);
            Map(x => x.Value);
        }
    }
}
