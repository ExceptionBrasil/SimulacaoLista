using Modelos.SimulacaoLista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys.Simulacao
{
    public static class PermissoesFactory
    {

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="modelView">The model view.</param>
        /// <returns></returns>
        public static PermissoesDaSimulacao BuildModel(PermissoesDaSimulacaoModelView modelView)
        {
            PermissoesDaSimulacao model = new PermissoesDaSimulacao()
            {
                Id = modelView.Id,
                NivelMaximo = modelView.NivelMaximo,
                DiscoverCustos= modelView.DiscoverCustos,
                DiscoverColunasDeCusto = modelView.DiscoverColunaDeCustos,
                FazAprovacao = modelView.FazAprovacao
            };

            return model;
        }


        /// <summary>
        /// Builds the model view.
        /// </summary>
        /// <param name="modelView">The model view.</param>
        /// <returns></returns>
        public static PermissoesDaSimulacaoModelView  BuildModelView(PermissoesDaSimulacao model)
        {
            PermissoesDaSimulacaoModelView modelView = new PermissoesDaSimulacaoModelView()
            {
                Id  = model.Id,
                NivelMaximo = model.NivelMaximo,
                DiscoverCustos = model.DiscoverCustos,
                Role = model.Role.Id,
                DiscoverColunaDeCustos = model.DiscoverColunasDeCusto,
                FazAprovacao = model.FazAprovacao,
                TipoDeCalculo = model.TipoDeCalculo.Id                
            };

            return modelView;
        }
    }
}
