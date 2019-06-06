using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SimulacaoLista
{
    public class SimulacaoPermissaoTipoDeCalculosMapping:ClassMap<SimulacaoPermissaoTipoDeCalculos>
    {
        public SimulacaoPermissaoTipoDeCalculosMapping()
        {
            Id(m => m.Id).GeneratedBy.Identity();
            Map(m => m.Descricao);
        }        
    }

}
