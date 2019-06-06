using Modelos.Admin;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.Admin
{
    public class UsuarioDAO
    {
        private ISession session;

        public UsuarioDAO(ISession s)
        {
            this.session = s;
        }

        public void Delete(Usuario usuario)
        {
            var tran = session.BeginTransaction();
            session.Delete(usuario);
            tran.Commit();
        }

        public void Alter(Usuario usuario)
        {
            var tran = session.BeginTransaction();
            session.Merge(usuario);
            tran.Commit();
        }

        public void Add(Usuario usuario)
        {
            var tran = session.BeginTransaction();
            session.Save(usuario);            
            tran.Commit();
        }

        public IList<Usuario> List()
        {
            return session.QueryOver<Usuario>()
                            .List();
                        
        }

        public Usuario GetById(int id)
        {
            var usuario = session.QueryOver<Usuario>()
                                .Where(u => u.Id == id)
                                .SingleOrDefault();
            return usuario;
        }


        public Usuario GetByLogin(string login)
        {
            var usuario = session.QueryOver<Usuario>()
                            .Where(u => u.Login == login)
                            .SingleOrDefault();
            return usuario;
        }

        public Usuario GetByNome(string nome)
        {
            var usuario = session.QueryOver<Usuario>()
                            .Where(u => u.Nome == nome)
                            .SingleOrDefault();
            return usuario;
        }

        public Usuario GetByEmail(string email)
        {
            var usuario = session.QueryOver<Usuario>()
                            .Where(u => u.Email == email)
                            .SingleOrDefault();
            return usuario;
        }
    }
}
