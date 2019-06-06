using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Formulas
{
    public class EstruturaProdutoMapping:ClassMap<EstruturaProduto>
    {
        public EstruturaProdutoMapping()
        {
            Id(e => e.Id).GeneratedBy.Identity();
            Map(e => e.Nivel);
            Map(e => e.Filial);
            Map(e => e.Componente);
            Map(e => e.DescricaoComponente);
            Map(e => e.Produto);
            Map(e => e.Tipo);
            Map(e => e.Unidade);
            Map(e => e.CentroCustoProduto);
            Map(e => e.CentroCustoComponente);
            Map(e => e.PercentualFormula);
            Map(e => e.Rendimento);
            Map(e => e.UltimoCustoMedio);            
            Map(e => e.CustoMateriPrima);
            Map(e => e.DespesaOperacional);
           
            
            
            

    }


}
}
