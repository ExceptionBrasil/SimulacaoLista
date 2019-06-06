using Modelos.Cadastros.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SimulacaoLista
{
    public class JsonSimulacao
    {

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Formula { get; set; }
        public string Familia { get; set; } //grupo do Produto no TOTVS
        public string CentroCusto { get; set; }
        public string DescricaoCCusto { get; set; }
        public double Rendimento { get; set; }
        public string Filial { get; set; }
        public int Niveis { get; set; }
        public string[,] Estrutura { get; set; }
        public double CustoEmbalagemPercent { get; set; }
        public double CustoEmbalagem { get; set; }
        public double CustoOperacional { get; set; }
        public double CustoIndustrial { get; set; }
        public double DespesasOperacionais { get; set; }
        public double DespesasOperacionaisCalculada { get; set; }
        public double CustoTotalDoProduto { get; set; }
        public double CustoTotalDoProdutoMargem { get; set; }
        public double MargemLucro { get; set; }
        public double PrecoBase { get; set; }
        public double PrecoBaseIcm7 { get; set; }
        public double PrecoBaseIcm12 { get; set; }
        public double PrecoBaseIcm18 { get; set; }

    }
}
