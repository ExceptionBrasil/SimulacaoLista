using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SimulacaoLista
{
    public class ParametrosDaSimulacao
    {

        private string codigoDoProduto;

        [Required]
        [Display(Name = "Código do Produto")]
        public string CodigoDoProduto
        {
            get
            {
                if (codigoDoProduto != null)
                {
                    return codigoDoProduto.PadRight(15);
                }
                return "";
            }
            set
            {
                this.codigoDoProduto = value;
            }
        }

        [Display(Name = "Fórmula Base")]
        public string FormulaBase { get; set; }

        [Display(Name="Fórmula da Simulação")]
        public string FormulaSimulacao { get; set; }

        [Required]
        [Display(Name = "Filial")]
        public string Filial { get; set; }

    }
}
