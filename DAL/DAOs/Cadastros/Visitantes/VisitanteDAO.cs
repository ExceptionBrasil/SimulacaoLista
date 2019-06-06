using Modelos.Cadastros.Visitantes;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.Cadastros.Visitantes
{
    /// <summary>
    /// Camada respossável por acessar os dados do visitante
    /// </summary>
    public class VisitanteDAO
    {
        private ISession session;
        /// <summary>
        /// Initializes a new instance of the <see cref="VisitanteDAO"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public VisitanteDAO(ISession s)
        {
            this.session = s;
        }


        /// <summary>
        /// Obtem um visitante pelo ID
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Visitante GetById(int id)
        {
            var visitante = session.QueryOver<Visitante>()
                                .Where(x => x.Id == id)                                                                
                                .SingleOrDefault();
            return visitante;
        }

        /// <summary>
        /// Retorna uma lista de visitantes que possuem nomes semelhantes há <paramref name="nome"/>
        /// </summary>
        /// <param name="nome">The nome.</param>
        /// <returns></returns>
        public IList<Visitante> GetByNome(string nome)
        {
            var visitantes = session.QueryOver<Visitante>()
                                    .WhereRestrictionOn(x=> x.Nome)
                                    .IsLike("%" + nome + "%")
                                    .List();
            return visitantes;
        }

        /// <summary>
        /// Obtem um visitante pelo seu documento  (CPF)
        /// </summary>
        /// <param name="documento">The documento.</param>
        /// <returns></returns>
        public Visitante GetByDocumento(string documento)
        {
            var visitante = session.QueryOver<Visitante>()
                                   .Where(x => x.Documento == documento)
                                   .SingleOrDefault();
            return visitante;
        }

        /// <summary>
        /// Obtem todos os visitantes que tem o mesmo Documento, mas não é o visitante consultado
        /// </summary>
        /// <param name="vis">The vis.</param>
        /// <returns></returns>
        public IList<Visitante> GetAllSimilarsByDocumento(Visitante vis)
        {
            IList<Visitante> visitantes = session.QueryOver<Visitante>()
                                                .Where(x => x.Documento == vis.Documento)
                                                .And(x => x.Id != vis.Id)
                                                .List();
            return visitantes;
        }

        /// <summary>
        /// Obtem um visitante que contenha parte do documento  (CPF) 
        /// </summary>
        /// <param name="documento">The documento.</param>
        /// <returns></returns>
        public IList<Visitante> GetByDocumentoLike(string documento)
        {

            var visitante = session.QueryOver<Visitante>()
                                   .WhereRestrictionOn(x => x.Documento).IsLike("%" + documento + "%")
                                   .List();



            return visitante;
        }
        /// <summary>
        /// Adiciona um novo visitante
        /// </summary>
        /// <param name="visitante">The visitante.</param>
        public void Add(Visitante visitante)
        {

            //visitante.Sequencial = this.GetNextSequence();

            var tran = session.BeginTransaction();
            session.Save(visitante);
            tran.Commit();

            session.CreateSQLQuery("exec usp_ReorderVisitante")
                .ExecuteUpdate();
                /*
         db.CreateSQLQuery("exec pSetClassForTeacher 
        @TeacherId=:TeacherId, @ClassId=:ClassId")
        .SetParameter("TeacherId", 3)
        .SetParameter("ClassId", 84)
        .ExecuteUpdate();    
             */
        }

        /// <summary>
        /// Apaga um  visitante
        /// </summary>
        /// <param name="visitante">The visitante.</param>
        public void Delete(Visitante visitante)
        {
            var tran = session.BeginTransaction();
            session.Delete(visitante);
            tran.Commit();
        }

        /// <summary>
        /// Altera um visitante
        /// </summary>
        /// <param name="visitante">The visitante.</param>
        public void Alter(Visitante visitante)
        {
            visitante.Sequencial = this.GetNextSequence();
            var tran = session.BeginTransaction();
            session.Merge(visitante);
            tran.Commit();
        }

        /// <summary>
        /// Lista todos visitantes cadastrados
        /// </summary>
        /// <returns></returns>
        public IList<Visitante> List()
        {
            var visitantes = session.QueryOver<Visitante>()
                                    .List();
            return visitantes;
        }


        /// <summary>
        /// Lista os usuários com controle de paginação 
        /// </summary>
        /// <param name="pagina">The pagina.</param>
        /// <param name="itensPorPagina">The itens por pagina.</param>
        /// <returns></returns>
        public async Task<IList<Visitante>> List(int pagina, int itensPorPagina)
        {

            int ate = pagina * itensPorPagina;
            int de = ate - itensPorPagina; 


            IList<Visitante> lista = await session.QueryOver<Visitante>()
                                    .Where(u => u.Sequencial > de && u.Sequencial <= ate)
                                    .OrderBy(u => u.Nome).Desc                                    
                                    .ListAsync();
                                    
            return lista;
        }

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return session.QueryOver<Visitante>()
                .RowCount();
        }

        /// <summary>
        /// Obtem a próxima sequencia de Visitante
        /// </summary>
        /// <returns></returns>
        private int GetNextSequence()
        {
            int output = session.Query<Visitante>().Max(v => v.Sequencial) + 1;
            return output;
        }
    }
}
