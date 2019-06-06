using Modelos.Admin.Acessos;
using Modelos.SimulacaoLista;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.SimulacaoLista
{
    public class PermissoesDaSimulacaoDAO
    {
        private ISession session;

        public PermissoesDaSimulacaoDAO(ISession s)
        {
            this.session = s;
        }

        /// <summary>
        /// Retorna uma Permissao pela role do usuário
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public PermissoesDaSimulacao GetByRole(Role role)
        {
            var permissao = session.QueryOver<PermissoesDaSimulacao>()
                                   .JoinQueryOver<Role>(r => r.Role)
                                   .Where(r=> r.Id==role.Id)
                                   .SingleOrDefault();
            return permissao;
                                   
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public PermissoesDaSimulacao GetById(int id)
        {
            var permissao = session.QueryOver<PermissoesDaSimulacao>()
                                   .Where(r => r.Id == id)
                                   .SingleOrDefault();
            return permissao;

        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        public IList<PermissoesDaSimulacao> List()
        {
            var permissoes = session.QueryOver<PermissoesDaSimulacao>().List();

            return permissoes;
        }

        /// <summary>
        /// Adiciona uma nova permissão 
        /// </summary>
        /// <param name="permissao">The permissao.</param>
        public void Add(PermissoesDaSimulacao permissao)
        {
            ITransaction tran = session.BeginTransaction();
            session.Save(permissao);
            tran.Commit();
        }

        /// <summary>
        /// Atualiza uma nova permissão
        /// </summary>
        /// <param name="permissao">The permissao.</param>
        public void Update(PermissoesDaSimulacao permissao)
        {
            ITransaction tran = session.BeginTransaction();
            session.Merge(permissao);
            tran.Commit();
        }

        /// <summary>
        /// Deleta uma nova permissão
        /// </summary>
        /// <param name="permissao">The permissao.</param>
        public void Delete(PermissoesDaSimulacao permissao)
        {
            ITransaction tran = session.BeginTransaction();
            session.Delete(permissao);
            tran.Commit();
        }

    }
}
