using FluentNHibernate.Mapping;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Cadastros.Produtos
{
    public class ProdutoMapping : ClassMap<Produto>
    {
        public ProdutoMapping()
        {            
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.Codigo);
            Map(p => p.Descricao);
            Map(p => p.Formula);
            Map(p => p.Familia);
            Map(p => p.CentroCusto);
            Map(p => p.DescricaoCCusto);
            Map(p => p.Rendimento);
            Map(p => p.Filial);            
            Map(p => p.CustoEmbalagemPercent);
            Map(p => p.CustoEmbalagem);
            Map(p => p.CustoOperacional);
            Map(p => p.CustoIndustrial);
            Map(p => p.DespesasOperacionais);            
            Map(p => p.CustoTotalDoProduto);
            Map(p => p.CustoTotalDoProdutoMargem);
            Map(p => p.MargemLucro);
            Map(p => p.DataDeAprovacao);
            Map(p => p.DataDeCriacao);
            Map(p => p.Md5Aprovacao);
            Map(p => p.Aprovado).Default("0").Not.Nullable();
            References(p => p.UsuarioDeAprovacao);
            References(p => p.UsuarioDeCriacao);
            HasMany(p => p.Estrutura).Cascade.All();
            Map(p => p.NomeDaSimulacao);
        }

    }
}
