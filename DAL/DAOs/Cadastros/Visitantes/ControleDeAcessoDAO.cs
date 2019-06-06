using Modelos.Cadastros.Visitantes;
using Modelos.Relatorios.Parametros;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.Cadastros.Visitantes
{
    /// <summary>
    /// Classe de controle de camada de acesso ao cadastro de visitantes
    /// </summary>
    public class ControleDeAcessoDAO
    {

        private ISession session;
        /// <summary>
        /// Classe contrutora <see cref="ControleDeAcessoDAO"/> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public ControleDeAcessoDAO(ISession s)
        {
            this.session = s;

        }

        /// <summary>
        /// Adiciona um novo controle de acesso
        /// </summary>
        /// <param name="acesso">The acesso.</param>
        public void Add(ControleDeAcesso acesso)
        {
            var tran = session.BeginTransaction();
            session.Save(acesso);
            tran.Commit();
        }

        /// <summary>
        /// Exclui um controle de acesso
        /// </summary>
        /// <param name="acesso">The acesso.</param>
        public void Delete(ControleDeAcesso acesso)
        {
            var tran = session.BeginTransaction();
            session.Delete(acesso);
            tran.Commit();
        }

        /// <summary>
        /// Faz a alteração do acesso
        /// </summary>
        /// <param name="acesso">The acesso.</param>
        public void Alter(ControleDeAcesso acesso)
        {
            var tran = session.BeginTransaction();
            session.Merge(acesso);
            tran.Commit();
        }

        /// <summary>
        /// Obtem pelo id um acesso
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ControleDeAcesso GetById(int id)
        {
            var acesso = session.QueryOver<ControleDeAcesso>()
                                   .Where(x => x.Id == id)
                                   .SingleOrDefault();
            return acesso;
        }

        /// <summary>
        /// Lista todos os acessos em ordem de incluzão
        /// </summary>
        /// <returns></returns>
        public IList<ControleDeAcesso> List()
        {
            var acessos = session.QueryOver<ControleDeAcesso>()
                                .List();
            return acessos;
        }

        /// <summary>
        /// Lists the specified resgistros.
        /// </summary>
        /// <param name="resgistros">The resgistros.</param>
        /// <returns></returns>
        public IList<ControleDeAcesso> List(int resgistros)
        {
            var acessos = session.QueryOver<ControleDeAcesso>()
                                .OrderBy(x => x.DataSaida).Desc
                                .Take(resgistros)
                                .List();

            return acessos;
        }

        /// <summary>
        /// Lista todos os visitantes por ordem descendente
        /// </summary>
        /// <returns></returns>
        public IList<ControleDeAcesso> ListOrderByDesc()
        {
            var acessos = session.QueryOver<ControleDeAcesso>()
                                  .OrderBy(x => x.DataEntrada).Desc
                                  .List();


            return acessos;
        }

        /// <summary>
        /// Lista todos os acesso sem data de saída ordenados por data de entrada
        /// Apenas os 1000 primeiros registros
        /// </summary>
        /// <returns></returns>
        public IList<ControleDeAcesso> AcessosSemDataDeSaida()
        {
            var acessos = session.QueryOver<ControleDeAcesso>()
                                 .Where(x => x.DataSaida == null)
                                 .OrderBy(x => x.DataEntrada).Desc
                                 .Take(1000)
                                 .List();
            return acessos;
        }

        /// <summary>
        /// Gera dados do relatório de acesso
        /// </summary>
        /// <param name="parametros">The parametros.</param>
        /// <returns></returns>
        public IList<ControleDeAcesso> Relatorio(RelAcessosParametros parametros)
        {

            IList<ControleDeAcesso> acessos;
            IList<ControleDeAcesso> acessoVisitado= new List<ControleDeAcesso>(); 
            IList<ControleDeAcesso> acessoVisitante= new List<ControleDeAcesso>() ;
            IList<ControleDeAcesso> acessosData = new List<ControleDeAcesso>(); 
            IList<ControleDeAcesso> acessoEmpresa = new List<ControleDeAcesso>(); 

         
            if (parametros.Visitado != null)
            {


                acessoVisitado = session.QueryOver<ControleDeAcesso>()
                                              .WhereRestrictionOn(v => v.Visitado)
                                                  .IsLike(parametros.Visitado, MatchMode.Anywhere)
                                              .List();
            }

            if (parametros.Empresa != null)
            {


                acessoEmpresa = session.QueryOver<ControleDeAcesso>()
                                              .JoinQueryOver(visitante => visitante.Visitante)
                                              .WhereRestrictionOn(visitante => visitante.Empresa)                                              
                                                  .IsLike(parametros.Empresa, MatchMode.Anywhere)
                                              .List();
            }
            if (parametros.Visitante !=null){
                acessoVisitante = session.QueryOver<ControleDeAcesso>()
                                    .JoinQueryOver(visitente => visitente.Visitante)
                                       .WhereRestrictionOn(visitante => visitante.Nome)
                                           .IsLike(parametros.Visitante, MatchMode.Anywhere)
                                   .List();
            }



            if (parametros.DataEntradaAte != null
               || parametros.DataEntradaDe != null
               || parametros.DataSaidaAte != null
               || parametros.DataSaidaDe != null)
            {

                if (parametros.DataEntradaDe == null)
                {
                    parametros.DataEntradaDe = DateTime.MinValue;
                }
                if (parametros.DataEntradaAte == null)
                {
                    parametros.DataEntradaAte = DateTime.MaxValue;
                }
                if (parametros.DataSaidaDe == null)
                {
                    parametros.DataSaidaDe = DateTime.MinValue;
                }
                if (parametros.DataSaidaAte == null)
                {
                    parametros.DataSaidaAte = DateTime.MaxValue;
                }


                acessosData = session.QueryOver<ControleDeAcesso>()
                                .WhereRestrictionOn(a => a.DataEntrada)
                                    .IsBetween(parametros.DataEntradaDe).And(parametros.DataEntradaAte)
                                .WhereRestrictionOn(a => a.DataSaida)
                                    .IsBetween(parametros.DataSaidaDe).And(parametros.DataSaidaAte)
                                .List();

                
                
            }

           
            
            acessos = acessosData.Union(acessoVisitado)
                                 .Union(acessoVisitante)
                                 .Union(acessoEmpresa)
                                 .ToList();
            return acessos;

        }
    }
}
