using Modelos.Admin;
using Modelos.Custos.Produtos;
using Modelos.Formulas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Cadastros.Produtos
{
    public class Produto : ICustoProduto
    {
        public virtual int Id { get; set; }
        public virtual string  NomeDaSimulacao { get; set; }
        public virtual string Codigo { get; set; }              
        public virtual string Descricao { get; set; }              
        public virtual string Formula { get; set; }
        /// <summary>
        /// Representa o grupo de produto no TOTVS
        /// </summary>
        /// <value>
        /// The familia.
        /// </value>
        /// <example>
        /// Campo de referência 
        /// <code>
        /// SELECT B1_GRUPO FROM SB1010 
        /// </code>
        /// </example>
        public virtual string Familia { get; set; } 
        public virtual string CentroCusto { get; set; }
        public virtual string DescricaoCCusto { get; set; }                
        public virtual double Rendimento { get; set; }                
        public virtual string Filial { get; set; }        
        public virtual int Niveis
        {
            get
            {
                return (from estru in this.Estrutura
                        select estru.Nivel)
                         .Distinct()
                         .Count();
            }
        }
        public virtual IList<EstruturaProduto> Estrutura { get; set; }        
        public virtual double CustoEmbalagemPercent { get; set; }        
        public virtual double CustoEmbalagem { get; set; }        
        public virtual double CustoOperacional { get; set; }        
        public virtual double CustoIndustrial { get; set; }        
        public virtual double DespesasOperacionais { get; set; }        
        public virtual double DespesasOperacionaisCalculada
        {
            get
            {
                return this.CustoIndustrial * (this.DespesasOperacionais / 100);
            }            
        }
              
        public virtual double CustoTotalDoProduto { get; set; }
        public virtual double CustoTotalDoProdutoMargem
        {
            get
            {
                return this.CustoTotalDoProduto * 1.05; //Agrega sempre + 5%
            }
            set { }
        }

        public virtual double MargemLucro { get; set; }

        public virtual double PrecoBase
        {
            get
            {
                return this.CustoTotalDoProdutoMargem / ((100 - this.MargemLucro)/100);
            }
        }

        public virtual double PrecoBaseIcm7
        {
            get
            {
                return this.PrecoBase /0.8375; //100%-(9.25%PisCof + 7%Icms)
            }
        }

        public virtual double PrecoBaseIcm12
        {
            get
            {
                return this.PrecoBase / 0.7875; //100%-(9.25%PisCof + 12% Icms)
            }
        }

        public virtual double PrecoBaseIcm18
        {
            get
            {
                return this.PrecoBase / 0.7275; //100%-(9.25%PisCof + 18%Icms)
            }
        }

        public virtual DateTime DataDeCriacao { get; set; }
        public virtual DateTime DataDeAprovacao { get; set; }
        public virtual Usuario UsuarioDeCriacao { get; set; }
        public virtual Usuario UsuarioDeAprovacao { get; set; }
        public virtual bool Aprovado { get; set; }
        public virtual string Md5Aprovacao { get; set; }

        public virtual void CalculaTotalDoProduto()
        {
            this.CustoTotalDoProduto = this.CustoIndustrial + this.DespesasOperacionaisCalculada;
        }


        /// <summary>
        /// Obtem o Custo Total do Produto do Nível 1
        /// </summary>
        /// <returns></returns>
        public virtual double GetCustoTotalNivel(int nivel)
        {
            return this.Estrutura.Where(e => e.Nivel == nivel)
                         .Sum(e => e.CustoTotalComponente);
        }
        public virtual double GetCustoTotalNivelComRendimento(int nivel)
        {
            double subTotal =this.Estrutura.Where(e => e.Nivel == nivel)
                         .Sum(e => e.CustoTotalComponente);
            return subTotal / (this.Rendimento / 100);

        }
        /// <summary>
        /// Soma o Total de Percentual de Fórmula por Nivel
        /// </summary>
        /// <param name="nivel"></param>
        /// <returns></returns>
        public virtual double GetTotalPercentualNivel(int nivel)
        {
            return this.Estrutura.Where(e => e.Nivel == nivel)
                .Sum(e => e.PercentualFormula);
        }
      
        /// <summary>
        /// Calcula o custo Industrial
        /// </summary>
        public virtual void CalculaCustoIndustrial()
        {
            this.CustoIndustrial =  this.CustoEmbalagem + 
                                    this.CustoOperacional + 
                                    (GetCustoTotalNivel(1)/(this.Rendimento/100));
        }

        /// <summary>
        /// Retorna a descrição do Produto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Descricao;
        }

    }


}
