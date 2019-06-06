using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SimulacaoLista
{
    public class JsonSimulacaoItens
    {
        public virtual int Id { get; set; }
        public virtual int Nivel { get; set; }
        public virtual string Filial { get; set; }
        public virtual string Componente { get; set; }
        public virtual string DescricaoComponente { get; set; }
        public virtual string Produto { get; set; }
        public virtual string Tipo { get; set; }
        public virtual string Unidade { get; set; }
        public virtual string CentroCustoProduto { get; set; }
        public virtual string CentroCustoComponente { get; set; }
        public virtual double PercentualFormula { get; set; }
        public virtual double Rendimento { get; set; }
        public virtual double UltimoCustoMedio { get; set; }
        public virtual double CustoMedioUnitario { get; set; }
        public virtual double CustoMateriPrima { get; set; }
        public virtual double DespesaOperacional { get; set; }
        public virtual double CustoTotalComponente { get; set; }
    }
}
