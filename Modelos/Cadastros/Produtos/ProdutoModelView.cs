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
    public class ProdutoModelView
    {
        public virtual int Id { get; set; }

        [Display(Name = "Cod. Produto")]
        public virtual string Codigo { get; set; }

        [Display(Name = "Descrição")]
        [Required]
        public virtual string Descricao { get; set; }

        [Display(Name = "Fórmula")]
        public virtual string Formula { get; set; }

        [Display(Name = "Família")]
        [Required]
        public virtual string Familia { get; set; } //grupo do Produto no TOTVS

        [Display(Name = "C. Custo")]
        public virtual string CentroCusto { get; set; }

        [Display(Name = "C.C. Descrição")]
        public virtual string DescricaoCCusto { get; set; }

        private double rendimento;

        [Display(Name = "Rendimento")]
        [Required]
        [RegularExpression(@"^\d{0,3}(\,\d{1,2})?$")]
        public virtual double Rendimento
        {
            get => rendimento == null ?  rendimento:100;
            set
            {
                this.rendimento = value;
            }
        }

        [Display(Name = "Filial")]
        [Required]
        public virtual string Filial { get; set; }

        [Display(Name = "Níveis")]
        public virtual int Niveis { get; set; }

        [Display(Name = "Embalagem")]
        public virtual double CustoEmbalagemPercent { get; set; }

        [Display(Name = "Cust. Embalagem")]
        public virtual double CustoEmbalagem { get; set; }

        [Display(Name = "Cust. Operacional")]
        public virtual double CustoOperacional { get; set; }

        [Display(Name = "Cust. Industrial")]
        public virtual double CustoIndustrial { get; set; }

        [Display(Name = "Despesas Operacionais")]
        public virtual double DespesasOperacionais { get; set; }

        [Display(Name = "Total do Produto S/P")]
        public virtual double CustoTotalDoProduto { get; set; }

        [Display(Name = "Total do Produto C/P")]
        public virtual double CustoTotalDoProdutoMargem { get; set; }

        [Display(Name = "Margem")]
        public virtual double MargemLucro { get; set; }

        [Display(Name = "Preço Base")]
        public virtual double PrecoBase { get; set; }

        [Display(Name = "ICMS 7")]
        public virtual double PrecoBaseIcm7 { get; set; }

        [Display(Name = "ICMS 12")]
        public virtual double PrecoBaseIcm12 { get; set; }

        [Display(Name = "ICMS 18")]
        public virtual double PrecoBaseIcm18 { get; set; }

        [Display(Name = "Nome da Simulação")]
        public virtual string NomeDaSimulacao { get; set; }

    }


}
