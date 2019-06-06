using Modelos.Mensageria;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.Mensageria
{
    public class MensagensTruckDAO
    {
        private ISession session;
        
        public MensagensTruckDAO(ISession s)
        {
            this.session = s;
        }


        public void Add(MensagensTruck mensagem)
        {
            var tran = session.BeginTransaction();
            session.Save(mensagem);
            tran.Commit();
        }

        public void Alter(MensagensTruck mensagem)
        {
            var tran = session.BeginTransaction();
            session.Merge(mensagem);
            tran.Commit();
        }

        public void Delete(MensagensTruck mensagem)
        {
            var tran = session.BeginTransaction();
            session.Delete(mensagem);
            tran.Commit();
        }

        public IList<MensagensTruck> List()
        {
            IList<MensagensTruck> lista = session.QueryOver<MensagensTruck>()
                                                 .List();
            return lista;
        }
    }
}
