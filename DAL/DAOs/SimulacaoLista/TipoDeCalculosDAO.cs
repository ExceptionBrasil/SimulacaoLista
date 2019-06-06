using Modelos.SimulacaoLista;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.SimulacaoLista
{
    public class TipoDeCalculosDAO
    {
        private ISession session;

        public TipoDeCalculosDAO(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        public IList<SimulacaoPermissaoTipoDeCalculos> List()
        {
            return session.QueryOver<SimulacaoPermissaoTipoDeCalculos>().List();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public SimulacaoPermissaoTipoDeCalculos GetById(int id)
        {
            SimulacaoPermissaoTipoDeCalculos tipo = session.QueryOver<SimulacaoPermissaoTipoDeCalculos>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
            return tipo;
        }
    }
}
