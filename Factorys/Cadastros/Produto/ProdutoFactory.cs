using Modelos.Cadastros.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys.Cadastros.Produto
{
    public static class ProdutoFactory
    {
        /// <summary>
        /// Cria uma Model com base em uma Model View
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static Modelos.Cadastros.Produtos.Produto BuildModel(ProdutoModelView p)
        {
            Modelos.Cadastros.Produtos.Produto model = new Modelos.Cadastros.Produtos.Produto()
            {
                CentroCusto = p.CentroCusto,
                Codigo = p.Codigo,
                CustoEmbalagem = p.CustoEmbalagem,
                CustoEmbalagemPercent = p.CustoEmbalagemPercent,
                CustoIndustrial = p.CustoIndustrial,
                CustoOperacional = p.CustoOperacional,
                CustoTotalDoProduto = p.CustoTotalDoProduto,
                Descricao = p.Descricao,
                DescricaoCCusto = p.DescricaoCCusto,
                DespesasOperacionais = p.DespesasOperacionais,
                Familia = p.Familia,
                Filial =p.Filial,
                Formula = p.Formula,
                Id =p.Id,
                MargemLucro = p.MargemLucro,
                Rendimento = p.Rendimento
            };
            return model;
        }

        public static ProdutoModelView BuildModelView(Modelos.Cadastros.Produtos.Produto p)
        {
            ProdutoModelView modelView = new ProdutoModelView()
            {
                CentroCusto = p.CentroCusto,
                Codigo = p.Codigo,
                CustoEmbalagem = p.CustoEmbalagem,
                CustoEmbalagemPercent = p.CustoEmbalagemPercent,
                CustoIndustrial = p.CustoIndustrial,
                CustoOperacional = p.CustoOperacional,
                CustoTotalDoProduto = p.CustoTotalDoProduto,
                Descricao = p.Descricao,
                DescricaoCCusto = p.DescricaoCCusto,
                DespesasOperacionais = p.DespesasOperacionais,
                Familia = p.Familia,
                Filial = p.Filial,
                Formula = p.Formula,
                Id = p.Id,
                MargemLucro = p.MargemLucro,
                Rendimento = p.Rendimento
            };
            return modelView;

        }
    }
}
