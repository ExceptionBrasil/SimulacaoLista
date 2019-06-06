using Modelos.Admin.Acessos;
using Modelos.Helps;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.Helps
{
    public class HelpDAO
    {
        private ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpDAO"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public HelpDAO(ISession s)
        {
            this.session = s;
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        public IList<Help> List()
        {
            return session.QueryOver<Help>().List();
        }

        public IList<Help> ListDescend()
        {
            return session.QueryOver<Help>().OrderBy(x => x.Id).Desc.List();
        }

        public Help GetById(int id)
        {
            var help = session.QueryOver<Help>()
                              .Where(h => h.Id == id)
                              .SingleOrDefault();
            return help;

        }

        /// <summary>
        /// Otem as últimas novidades geral
        /// </summary>
        /// <returns></returns>
        public IList<Help> GetNews()
        {
            var news = session.QueryOver<Help>()
                              .Where(h => h.DataExpiracao <= DateTime.Now)
                              .List();
            return news;

        }

        /// <summary>
        /// Otem as últimas novidades geral por role
        /// </summary>
        /// <returns></returns>
        public IList<Help> GetNews(Role role)
        {
            var news = session.QueryOver<Help>()
                              .Where(h => h.DataExpiracao >= DateTime.Now)
                              .And(h=> h.DataPublicacao !=null)
                              .JoinQueryOver<Role>(r => r.Role)
                                .Where(r=> r.Id==role.Id)                                
                              .List();
            var newsGeral = session.QueryOver<Help>()
                              .Where(h => h.DataExpiracao >= DateTime.Now)
                              .And(h => h.DataPublicacao != null && h.AvisoGeral)
                                
                              .List();
            news.Union(newsGeral);
            return news;

        }
        /// <summary>
        /// Adds the specified h.
        /// </summary>
        /// <param name="h">The h.</param>
        public void Add(Help h)
        {
            ITransaction tran = session.BeginTransaction();
            session.Save(h);
            tran.Commit();
        }

        /// <summary>
        /// Alters the specified h.
        /// </summary>
        /// <param name="h">The h.</param>
        public void Alter(Help h)
        {
            ITransaction tran = session.BeginTransaction();
            session.Merge(h);
            tran.Commit();
        }

        /// <summary>
        /// Deletes the specified h.
        /// </summary>
        /// <param name="h">The h.</param>
        public void Delete(Help h)
        {
            ITransaction tran = session.BeginTransaction();
            session.Delete(h);
            tran.Commit();
        }

    }
}
