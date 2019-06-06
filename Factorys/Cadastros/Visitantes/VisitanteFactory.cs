using Modelos.Cadastros.Visitantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys.Cadastros.Visitantes
{
    public static class VisitanteFactory
    {
        public static Visitante BuildModel (AcessoModelView modelView)
        {
            Visitante visitante = new Visitante();

            visitante.Documento = modelView.Documento;
            visitante.Empresa = modelView.Empresa;           
            visitante.Nome = modelView.Nome;
            visitante.Rg = modelView.Rg;
            visitante.Telefone = modelView.Telefone;
            visitante.Id = modelView.IdVisitante;
            visitante.FotoBase64 = modelView.Foto;
            return visitante;

        }
    }
}
