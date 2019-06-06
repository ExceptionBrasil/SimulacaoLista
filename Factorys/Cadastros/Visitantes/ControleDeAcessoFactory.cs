using Modelos.Cadastros.Visitantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys.Cadastros.Visitantes
{
    /// <summary>
    /// Factory resposável por conversões de Controle de Acesso
    /// </summary>
    public static class ControleDeAcessoFactory
    {
        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="modelView">The model view.</param>
        /// <returns></returns>
        public static ControleDeAcesso BuildModel(AcessoModelView modelView)
        {
            ControleDeAcesso controleDeAcesso = new ControleDeAcesso();

            controleDeAcesso.DataEntrada = modelView.DataEntrada;
            controleDeAcesso.DataSaida = modelView.DataSaida;
            controleDeAcesso.Id = modelView.IdAcesso;
            controleDeAcesso.Motivo = modelView.Motivo;
            controleDeAcesso.IdCartao = modelView.IdCartao;
            controleDeAcesso.Visitado = modelView.Visitado;
            controleDeAcesso.Visitante = new Visitante();
            

            return controleDeAcesso;

        }

        /// <summary>
        /// Builds the model view.
        /// </summary>
        /// <param name="acesso">The acesso.</param>
        /// <returns></returns>
        public static AcessoModelView BuildModelView(ControleDeAcesso acesso)
        {
            AcessoModelView modelView = new AcessoModelView()
            {
                DataEntrada = acesso.DataEntrada,
                DataSaida = acesso.DataSaida,
                IdAcesso = acesso.Id,
                IdCartao = acesso.IdCartao,
                Motivo = acesso.Motivo,
                IdVisitante = acesso.Visitante.Id,
                Documento = acesso.Visitante.Documento,
                Empresa = acesso.Visitante.Empresa,
                Nome = acesso.Visitante.Nome,
                Rg = acesso.Visitante.Rg,
                Telefone = acesso.Visitante.Telefone,
                Visitado = acesso.Visitado,
                Foto = ""

            };

            return modelView;
        }

        /// <summary>
        /// Builds the model view list.
        /// </summary>
        /// <param name="acesso">The acesso.</param>
        /// <returns></returns>
        public static IList<AcessoModelView> BuildModelViewList(IList<ControleDeAcesso> model)
        {
            IList<AcessoModelView> acessos = new List<AcessoModelView>();

            foreach (var item in model)
            {
                acessos.Add(BuildModelView(item));
            }

            return (acessos);
        }
    }
}
