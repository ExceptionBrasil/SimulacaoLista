using Modelos.Cadastros.Visitantes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Relatorios.Parametros
{
    public class RelAcessosParametros
    {
        
        [Display(Name = "Data de Entrada De")]        
        public virtual DateTime? DataEntradaDe { get; set; }

        
        [Display(Name = "Data de Entrada Ate")]
        public virtual DateTime? DataEntradaAte { get; set; }
        
        [Display(Name = "Data de Saída De")]
        public virtual DateTime? DataSaidaDe { get; set; }

        [Display(Name = "Data de Saída Ate")]
        public virtual DateTime? DataSaidaAte { get; set; }

        
        [Display(Name = "Visitante")]        
        public virtual string Visitante { get; set; }               


        [Display(Name ="Visitado")]
        public virtual string Visitado { get; set; }

        [Display(Name ="Gerar PDF?")]
        public virtual bool GerarPdf { get; set; }

        [Display(Name = "Informe a Empresa, ou parte do nome")]
        public virtual string Empresa { get; set; }
        
    }
}
