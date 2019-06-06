using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Custos.Produtos
{
    public interface ICustoProduto 
    {
        double CustoEmbalagemPercent { get; set; }
        double CustoEmbalagem { get; set; }
        double CustoOperacional { get; set; }
        double CustoIndustrial { get; set; }
        double DespesasOperacionais { get; set; }
        double DespesasOperacionaisCalculada { get; }    
        double CustoTotalDoProduto { get; set; }
        double CustoTotalDoProdutoMargem { get; }
        double MargemLucro { get; set; }
        double PrecoBase {get;}
        double PrecoBaseIcm7{get;}
        double PrecoBaseIcm12{get;}
        double PrecoBaseIcm18{get;}
        void CalculaCustoIndustrial();       
        void CalculaTotalDoProduto();
    }

  
}
