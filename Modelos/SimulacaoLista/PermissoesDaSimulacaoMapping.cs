using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SimulacaoLista
{
    public class PermissoesDaSimulacaoMapping:ClassMap<PermissoesDaSimulacao>
    {
        public PermissoesDaSimulacaoMapping()
        {
            Id(p => p.Id);
            Map(p => p.NivelMaximo);
            References(p => p.Role);
            Map(p => p.DiscoverCustos);           
            Map(p => p.DataDeCriacao);
            Map(p => p.DiscoverColunasDeCusto);
            Map(p => p.FazAprovacao);
            References(p => p.TipoDeCalculo)
                .Cascade.All();               
        }
    }
}
