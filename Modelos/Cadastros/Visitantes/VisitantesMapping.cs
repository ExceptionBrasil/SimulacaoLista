using FluentNHibernate.Mapping;
using Modelos.Cadastros.Visitantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Mappings.Cadastros.Visitantes
{
    public class VisitantesMapping:ClassMap<Visitante>
    {
        public VisitantesMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Nome);
            Map(x => x.Documento).Unique();
            Map(x => x.Telefone);
            Map(x => x.Empresa);            
            Map(x => x.Rg);
            Map(x => x.Bloqueado);
            Map(x => x.Obs);
            Map(x => x.Sequencial);
            Map(x => x.FotoId);
            Map(x => x.FotoPath);
            Map(x => x.FotoFile);
            Map(x => x.FotoBase64).CustomSqlType("VARCHAR(MAX)").LazyLoad();
        }
    }
}
