using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SimulacaoLista
{
    public class PermissoesDaSimulacaoModelView
    {
        public virtual int Id { get; set; }

        [Required]
        [Display(Name ="Regra")]
        public virtual int Role { get; set; }

        [Required]
        [Display(Name = "Qual é máximo visível?")]
        public virtual int  NivelMaximo { get; set; }

        [Required]
        [Display(Name = "Visualiza detalhes de Custos?")]
        public virtual bool DiscoverCustos { get; set; }
        
        [Required]
        [Display(Name ="Visualiza as colunas de Custo?")]
        public virtual bool DiscoverColunaDeCustos { get; set; }

        [Required]
        [Display(Name = "Essa regra é aprovadora?")]
        public virtual bool FazAprovacao { get; set; }

        [Required]
        [Display(Name = "Tipo de custo Usado?")]
        public virtual int TipoDeCalculo { get;set; }
    }
}
