using Modelos.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys.Helps
{
    public static class HelpFactory
    {

        /// <summary>
        /// Builds the model view.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns></returns>
        public static HelpView BuildModelView(Help h)
        {
            HelpView helpView = new HelpView()
            {
                Id = h.Id,
                Role = h.Role.Id,
                Texto = h.Texto,
                DataExpiracao = h.DataExpiracao,
                DataPublicacao = h.DataExpiracao,
                AvisoGeral = h.AvisoGeral
            };

            return helpView;
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="hv">The hv.</param>
        /// <returns></returns>
        public static Help BuildModel(HelpView hv)
        {
            Help help = new Help()
            {
                Id = hv.Id,
                Texto = hv.Texto,
                DataExpiracao = hv.DataExpiracao,
                DataPublicacao = hv.DataPublicacao,
                AvisoGeral = hv.AvisoGeral
            };
            return help;
        }
    }
}
