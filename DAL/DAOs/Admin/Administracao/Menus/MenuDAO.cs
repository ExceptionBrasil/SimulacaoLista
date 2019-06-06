
using Modelos.Admin.Acessos;
using NHibernate;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Cadastros.Administracao.Menus
{
    
    public class MenuDAO
    {
        ISession session;
        public MenuDAO(ISession i)
        {
            this.session = i;
        }

        public Menu GetById(int id)
        {
            var menu = session.QueryOver<Menu>()
                               .Where(m => m.Id == id)
                               .SingleOrDefault();
            return menu;
        }

        /// <summary>
        /// Adiciona um novo menu
        /// </summary>
        /// <param name="m">The m.</param>
        public void Add(Menu m)
        {
            var tran = session.BeginTransaction();            
            session.Save(m);            
            tran.Commit();
        }

        /// <summary>
        /// Deleta um Menu específico
        /// </summary>
        /// <param name="m">The m.</param>
        public void Delete(Menu m)
        {
            var tran = session.BeginTransaction();
            session.Delete(m);
            tran.Commit();
        }


        /// <summary>
        /// Altera um menu específico
        /// </summary>
        /// <param name="m">The m.</param>
        public void Alter(Menu m)
        {
            var tran = session.BeginTransaction();
            session.Merge(m);
            tran.Commit();
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        public IList<Menu> List()
        {
            IList<Menu> menus = session.QueryOver<Menu>().List();
            return menus;
        }


        /// <summary>
        /// Recupera todos os Menus de um controller
        /// </summary>
        /// <param name="Controller">The controller.</param>
        /// <returns></returns>
        public IList<Menu> Recovery(string controller)
        {
            IList<Menu> menus = session.QueryOver<Menu>()
                                .Where(x=> x.Controller==controller)
                                .List();
                
            return menus;
        }


        /// <summary>
        /// Retorna todos os Menus com a mesma Action dentro do Controller
        /// </summary>
        /// <param name="Action">The action.</param>
        /// <returns></returns>
        public IList<Menu> Recovery(string controller, string action)
        {
            IList<Menu> menus = session.QueryOver<Menu>()
                                .Where(x => x.Controller == controller && x.Action == action)
                                .List();                
            return menus;
        }

        /// <summary>
        /// Recupera todos os Menus de uma Localização
        /// </summary>
        /// <param name="Controller">The controller.</param>
        /// <returns></returns>
        public  IList<Menu> RecoveryByLocation(string location)
        {
            IList<Menu> menus = session.QueryOver<Menu>()
                                .Where(x => x.Location == location).List();                
                
            return menus;
        }

        /// <summary>
        /// Recupera todos os Menus de um controller pela localização e Regra
        /// </summary>
        /// <param name="Controller">The controller.</param>
        /// <returns></returns>
        public  IList<Menu> RecoveryByLocation(string location, int role)
        {
          
                IList<Menu> menus = session.QueryOver<Menu>()
                                .Where(m => m.Location == location)
                                .JoinQueryOver<Role>(x => x.Role)
                                .Where(r => r.Value == role)
                                .List();
                return menus;
          
        }


        /// <summary>
        /// Retorna todos os Menus com a mesma Action 
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public  IList<Menu> RecoveryByAction(string action)
        {
            IList<Menu> menus = session.QueryOver<Menu>()
                                .Where(x => x.Action == action)
                                .List();
            return menus;
        }
    }
}
