using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SimulacaoLista
{
    public class PermissoesDaSimulacao
    {
        public virtual  int Id { get; set; }
        public virtual Role Role { get; set; } 
        public virtual int  NivelMaximo { get; set; }
        public virtual bool DiscoverCustos { get; set; }
        public virtual DateTime DataDeCriacao{ get; set; }        
        public virtual bool DiscoverColunasDeCusto { get; set; }        
        public virtual bool FazAprovacao { get; set; }
        public virtual SimulacaoPermissaoTipoDeCalculos TipoDeCalculo { get; set; }
        
    }

}
