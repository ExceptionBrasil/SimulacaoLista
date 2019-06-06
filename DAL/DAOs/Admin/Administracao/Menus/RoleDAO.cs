using Modelos.Admin.Acessos;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Cadastros.Administracao.Menus
{
    /// <summary>
    /// Classe de controle a camada de dados de Regras
    /// </summary>
    public class RoleDAO
    {
        private ISession session;

        public RoleDAO(ISession c)
        {
            this.session = c;
        }

        /// <summary>
        /// Obtem por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Role GetById(int id)
        {
            Role regra = session.QueryOver<Role>()
                        .Where(r => r.Id == id)
                        .SingleOrDefault();
            return regra;
        }

        /// <summary>
        /// Obtem uma Role pelo seu valor 
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Role.</returns>
        public Role GetByValue(int value)
        {
            Role regra = session.QueryOver<Role>()
                            .Where(r => r.Value == value)
                            .SingleOrDefault();
            return regra;
        }

        /// <summary>
        /// ILista todas as regras existentes
        /// </summary>
        /// <returns>ILista of roles</returns>
        public IList<Role> List()
        {
            IList<Role> regras = session.QueryOver<Role>()
                                        .List();
            return regras;
        }

        /// <summary>
        /// Adiciona uma nova regra
        /// </summary>
        /// <param name="r"></param>
        public void Add (Role r)
        {
            var tran = session.BeginTransaction();
            session.Save(r);
            tran.Commit();
        }

        /// <summary>
        /// Exclui uma regra
        /// </summary>
        /// <param name="r"></param>
        public void Delete(Role r)
        {
            var tran = session.BeginTransaction();
            session.Delete(r);
            tran.Commit();
        }

        /// <summary>
        /// Faz a alteração de uma regra
        /// </summary>
        /// <param name="r"></param>
        public void Alter(Role r)
        {
            var tran = session.BeginTransaction();
            session.Merge(r);
            tran.Commit();
        }

    }
}
