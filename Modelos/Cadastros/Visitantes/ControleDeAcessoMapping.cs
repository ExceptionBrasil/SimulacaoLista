using FluentNHibernate.Mapping;
using Modelos.Cadastros.Visitantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Mappings.Cadastros.Visitantes
{
    public class ControleDeAcessoMapping:ClassMap<ControleDeAcesso>
    {
        public ControleDeAcessoMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DataEntrada);
            Map(x => x.DataSaida);
            Map(x => x.Motivo);
            Map(x => x.IdCartao);
            Map(x => x.DataInclusao);
            Map(x => x.Visitado);
            References(x => x.Visitante);
            References(x => x.Usuario);
            

        }
    }
}
