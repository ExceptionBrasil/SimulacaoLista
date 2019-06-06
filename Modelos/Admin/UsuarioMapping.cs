using FluentNHibernate.Mapping;
using Modelos.Admin;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Mappings.Admin
{
    class UsuarioMapping:ClassMap<Usuario>
    {
        public UsuarioMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Nome);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.DataCriacao);
            Map(x => x.IsAdmin);
            Map(x => x.LastAcess);
            Map(x => x.Login);
            Map(x => x.Ativo);
            References(x => x.Role);
        }
    }
}
