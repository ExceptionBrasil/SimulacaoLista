using DAL.DAOs.Formulas;
using Factorys.Cadastros.Produto;
using Factorys.Tools;
using Modelos.Admin;
using Modelos.Cadastros.Produtos;
using Modelos.Custos.Produtos;
using Modelos.Formulas;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.Cadastros.Produtos
{
    public class ProdutoDAO
    {
        private SqlConnection conecxao;
        private ISession session;
        private EstruturaProdutoDAO estruturaDAO;

        public ProdutoDAO(SqlConnection connection, ISession s, EstruturaProdutoDAO estruDAO)
        {
            this.conecxao = connection;
            this.session = s;
            this.estruturaDAO = estruDAO;
        }

        /// <summary>
        /// Salva o produto 
        /// </summary>
        /// <param name="produto">The produto.</param>
        public void Save(Produto produto)
        {
            ITransaction tran = session.BeginTransaction();
            session.Save(produto);
            tran.Commit();
        }

        /// <summary>
        /// Atualiza o produto
        /// </summary>
        /// <param name="produto">The produto.</param>
        public void Update(Produto produto)
        {
            ITransaction tran = session.BeginTransaction();
            session.Merge(produto);
            tran.Commit();
        }


        /// <summary>
        /// Deleta o produto 
        /// </summary>
        /// <param name="produto">The produto.</param>
        public void Delete(Produto produto)
        {
            ITransaction tran = session.BeginTransaction();
            session.Delete(produto);
            tran.Commit();
        }

        /// <summary>
        /// Obtem um produto pelo Id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Produto GetById(int id)
        {
            var produto = session.QueryOver<Produto>()
                            .Where(p => p.Id == id)
                            .SingleOrDefault();
            return produto;
        }

        /// <summary>
        /// Lista as simulações pelo usuário
        /// </summary>
        /// <param name="usuario">The usuario.</param>
        /// <returns></returns>
        public IList<Produto> ListByUser(Usuario usuario)
        {
            var produtos = session.QueryOver<Produto>()
                                .OrderBy(p=> p.DataDeCriacao).Desc
                                .JoinQueryOver<Usuario>(u => u.UsuarioDeCriacao)
                                .Where(u => u.Id == usuario.Id)                                
                                .List();
            return produtos;
        }

        /// <summary>
        /// Aprova uma fórmula
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void AprovarById(int id,Usuario u)
        {
            var p = this.GetById(id);
            p.Aprovado = true;
            p.DataDeAprovacao = DateTime.Now;
            p.UsuarioDeAprovacao = u;
            p.Md5Aprovacao = p.GetHashCode()
                                         .ToString()
                                         .GetHashMD5();
            this.Update(p);
        }
        /// <summary>
        /// reprova uma fórmula pelo ID
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void ReprovarById(int id, Usuario u)
        {
            var p = this.GetById(id);
            p.Aprovado = false;
            p.DataDeAprovacao = DateTime.Now;
            p.UsuarioDeAprovacao = u;
            this.Update(p);
        }

        /// <summary>
        /// Retorna a lista de Simulações não parovadas
        /// </summary>
        /// <returns></returns>
        public IList<Produto> ListNonAproved()
        {
            var produtos = session.QueryOver<Produto>()
                                   .Where(p => p.UsuarioDeAprovacao==null)
                                   .OrderBy(p=> p.Id).Desc
                                   .List();
            return produtos;
        }
        /// <summary>
        /// Retorna a lista de Simulações  parovadas
        /// </summary>
        /// <returns></returns>
        public IList<Produto> ListAproved()
        {
            var produtos = session.QueryOver<Produto>()
                                   .Where(p => p.UsuarioDeAprovacao != null)
                                   .OrderBy(p => p.DataDeAprovacao).Desc
                                   .List();
            return produtos;
        }
        /// <summary>
        /// Retorna o Centro de Custo de um Produto cadastrado no SB1
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="filial"></param>
        /// <returns></returns>
        public  async Task<string> GetCentroCustoByCod(string codigo, string filial)
        {
            using(SqlCommand command = conecxao.CreateCommand())
            {
                string query = String.Format("SELECT B1_CC " +
                    " FROM SB1010 " +
                    " WHERE " +
                    " D_E_L_E_T_='' " +
                    " AND B1_FILIAL={0} " +
                    " AND B1_COD={1}",filial,codigo);
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = query;

                using(SqlDataReader leitor = await command.ExecuteReaderAsync())
                {
                    leitor.Read();
                    return leitor["B1_CC"].ToString();
                }
            }            
        }

        /// <summary>
        /// Obtem o produto e sua estrutura 
        /// </summary>
        /// <param name="codigo">The codigo.</param>
        /// <param name="filial">The filial.</param>
        /// <param name="formula">The formula.</param>
        /// <returns></returns>
        public async Task<Produto> GetByCodigoAsync(string codigo, string filial, string formula,int tipoCalculo)
        {
            if (String.IsNullOrEmpty(formula))
            {
                formula = LastFormula(codigo, filial);
            }


            Produto produto = new Produto();

            using (SqlCommand comando = conecxao.CreateCommand())
            {


                string query = String.Format(" SELECT B1_FILIAL, B1_COD,B1_DESC,B1_REVATU," +
                                      " B1_GRUPO,B1_CC,CTT_DESC01,ISNULL(Z01_REND,100) AS Z01_REND, " +
                                      " ISNULL(Z07_PROD, '') AS Z07_PROD, ISNULL(Z07_CC, '') AS Z07_CC, " +
                                      " ISNULL(Z07_GRUPO, '') AS Z07_GRUPO, ISNULL(Z07_CUSTO1, 0) AS Z07_CUSTO1, " +
                                      " ISNULL(Z07_PERC01, 0) AS Z07_PERC01, ISNULL(Z07_LUCRO, 0) AS Z07_LUCRO, " +
                                      " ISNULL(Z07_CEMBA, 0)AS Z07_CEMBA, ISNULL(Z07_VEMBA, 0) AS Z07_VEMBA, " +
                                      " ISNULL(Z07_CUSTF, 0) AS Z07_CUSTF " +
                                      " FROM SB1010 AS SB1 " +
                                      " JOIN CTT010 AS CTT ON CTT_CUSTO = B1_CC AND LEFT(B1_FILIAL,2)= LEFT(CTT_FILIAL, 2)" +
                                      " LEFT JOIN Z01010 AS Z01 ON Z01_FILIAL = B1_FILIAL AND Z01_COD = B1_COD " +
                                      " AND Z01_FORM = '{2}' AND Z01.D_E_L_E_T_ = ''" +
                                      " LEFT JOIN Z07010 AS Z07 ON Z07_FILIAL= B1_FILIAL AND Z07_GRUPO=B1_GRUPO AND Z07.D_E_L_E_T_=''" +
                                      " WHERE    SB1.D_E_L_E_T_ = ''    " +
                                      "AND B1_FILIAL = '{0}'    " +
                                      "AND B1_MSBLQL<>'1'    " +
                                      "AND B1_COD = '{1}' ", filial, codigo, formula);

                comando.CommandText = query;
                try
                {
                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        if (leitor.HasRows)
                        {
                            leitor.Read();

                            produto.CentroCusto = Convert.ToString(leitor["B1_CC"]);
                            produto.Codigo = codigo;
                            produto.Descricao = Convert.ToString(leitor["B1_DESC"]);
                            produto.DescricaoCCusto = Convert.ToString(leitor["CTT_DESC01"]);
                            produto.Familia = Convert.ToString(leitor["B1_GRUPO"]);
                            produto.Formula = Convert.ToString(leitor["B1_REVATU"]);
                            produto.Rendimento = Convert.ToDouble(leitor["Z01_REND"]);
                            produto.Filial = filial;
                            produto.CustoEmbalagemPercent = Convert.ToDouble(leitor["Z07_CEMBA"]);
                            produto.CustoEmbalagem = (produto.CustoEmbalagemPercent / 100) * Convert.ToDouble(leitor["Z07_VEMBA"]);
                            produto.CustoOperacional = Convert.ToDouble(leitor["Z07_CUSTO1"]);
                            produto.DespesasOperacionais = Convert.ToDouble(leitor["Z07_PERC01"]);
                            produto.MargemLucro = Convert.ToDouble(leitor["Z07_LUCRO"]);
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            produto.Estrutura = await estruturaDAO
                                .GetEstruturaAsync(codigo, filial, formula,tipoCalculo);

            produto.CalculaCustoIndustrial();
            produto.CalculaTotalDoProduto();

            return produto;
        }

        public Produto GenerateByFramilia(ProdutoModelView modelView)
        {
            Produto produto = ProdutoFactory.BuildModel(modelView);

            using (SqlCommand comando = conecxao.CreateCommand())
            {
                string query = String.Format(" SELECT  Z07_FILIAL,Z07_GRUPO,Z07_CUSTO1,Z07_PERC01 " +
                                             ", Z07_LUCRO, Z07_LUCRO, Z07_CEMBA, Z07_VEMBA, Z07_CUSTF" +
                                             " FROM Z07010 " +
                                             " WHERE Z07_FILIAL = '{0}'" +
                                             " AND Z07_GRUPO = '{1}' AND D_E_L_E_T_ = ''",
                                             modelView.Filial
                                             , modelView.Familia);
                comando.CommandText = query;
                comando.CommandType = System.Data.CommandType.Text;

                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        produto.CustoEmbalagemPercent = Convert.ToDouble(leitor["Z07_CEMBA"]);
                        produto.CustoEmbalagem = (produto.CustoEmbalagemPercent / 100) * Convert.ToDouble(leitor["Z07_VEMBA"]);
                        produto.CustoOperacional = Convert.ToDouble(leitor["Z07_CUSTO1"]);
                        produto.DespesasOperacionais = Convert.ToDouble(leitor["Z07_PERC01"]);
                        produto.MargemLucro = Convert.ToDouble(leitor["Z07_LUCRO"]);
                        produto.Rendimento = modelView.Rendimento;
                    }
                }

            }

            return produto;
        }




        /// <summary>
        /// Obtem todas as fórmulas cadastradas do Produto 
        /// </summary>
        /// <param name="produto">The produto.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IList<string> GetFormula(string produto, string filial)
        {
            IList<string> formulas = new List<string>();

            using (SqlCommand comando = conecxao.CreateCommand())
            {
                string query = String.Format("select DISTINCT G1_REVFIM" +
                                            " from SG1010 " +
                                            " WHERE D_E_L_E_T_ = ''" +
                                            " AND G1_FILIAL = '{0}'" +
                                            " AND G1_COD = '{1}'" +
                                            " AND G1_REVFIM <> 'ZZZ'"
                                            , filial, produto);
                comando.CommandText = query;

                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        formulas.Add(leitor["G1_REVFIM"].ToString());
                    }
                }
            }
            return formulas;

        }

        /// <summary>
        /// Obtem uma fórmula específica 
        /// </summary>
        /// <param name="produto">The produto.</param>
        /// <param name="filial">The filial.</param>
        /// <param name="formula">The formula.</param>
        /// <returns></returns>
        public IList<string> GetFormula(string produto, string filial, string formula)
        {
            IList<string> formulas = new List<string>();

            using (SqlCommand comando = conecxao.CreateCommand())
            {
                string query = String.Format("select DISTINCT G1_REVFIM" +
                                            " from SG1010 " +
                                            " WHERE D_E_L_E_T_ = ''" +
                                            " AND G1_FILIAL = '{0}'" +
                                            " AND G1_COD = '{1}'" +
                                            " AND G1_REVFIM = '{2}'"
                                            , filial, produto, formula);
                comando.CommandText = query;

                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        formulas.Add(leitor["G1_REVFIM"].ToString());
                    }
                }
            }
            return formulas;

        }

        /// <summary>
        /// Obtem uma lista de produtos pela string de pesquisa
        /// </summary>
        /// <param name="search">String de pesquisa</param>
        /// <returns></returns>
        public async Task<IList<string>> GetProdutoListAsync(string search, string filial)
        {

            IList<string> produtos = new List<string>();

            using (SqlCommand comando = conecxao.CreateCommand())
            {

                string query = String.Format(" SELECT TOP 15" +
                                      " SUBSTRING(B1_COD,1,15)+'-'+ SUBSTRING(B1_DESC,1,20) AS B1_COD " +
                                      " FROM SB1010   " +
                                      " WHERE  D_E_L_E_T_ = '' " +
                                      " AND B1_COD LIKE '%{0}%'  " +
                                      " AND B1_FILIAL='{1}' AND B1_MSBLQL<>'1' " +
                                      " ORDER BY  B1_COD ", search, filial);

                comando.CommandText = query;
                try
                {
                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        {
                            produtos.Add(leitor["B1_COD"].ToString());
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.Write(e.Message);
                }
            }

            return produtos;
        }

        /// <summary>
        /// Obtem a descrição do Produto buscando pelo código e Filial
        /// </summary>
        /// <param name="filial"></param>
        /// <param name="codigoProduto"></param>
        /// <returns></returns>
        public async Task<EstruturaProduto> GetDadosDaEstruturaAsync(string filial, string codigoProduto)
        {

            Produto produto = new Produto();
            EstruturaProduto estrutura = new EstruturaProduto();

            using (SqlCommand comando = conecxao.CreateCommand())
            {

                string query = String.Format("select B1_FILIAL,B1_TIPO," +
                                               " B1_DESC," +
                                               " B1_GRUPO,B1_UM, " +
                                               " ISNULL(Z06_CUSTO1,0) AS Z06_CUSTO1" +
                                            " from SB1010 SB1" +
                                               " LEFT JOIN Z06010 AS Z06 ON  Z06_PROD=B1_COD AND  Z06_FILIAL=B1_FILIAL" +
                                           " where" +
                                           " SB1.D_E_L_E_T_=''" +
                                           " AND B1_COD='{0}'" +
                                           " AND B1_FILIAL='{1}'", codigoProduto, filial);

                comando.CommandText = query;

                using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                {
                    while (leitor.Read())
                    {
                        estrutura.DescricaoComponente = leitor["B1_DESC"].ToString();
                        estrutura.Filial = leitor["B1_FILIAL"].ToString();
                        estrutura.Tipo = leitor["B1_TIPO"].ToString();
                        estrutura.Unidade = leitor["B1_UM"].ToString();
                        estrutura.CustoMateriPrima = Convert.ToDouble(leitor["Z06_CUSTO1"]);
                    }
                }
                if (estrutura.Tipo == "PA")
                {
                    produto = await this.GetByCodigoAsync(codigoProduto, filial, this.LastFormula(codigoProduto, filial),1);
                    estrutura.CustoMateriPrima = produto.GetCustoTotalNivelComRendimento(1);
                }
            }
            return estrutura;
        }

        /// <summary>
        /// Obtem a ultima fórmula do produto
        /// </summary>
        /// <param name="produto"></param>
        /// <param name="filial"></param>
        /// <returns></returns>
        public string LastFormula(string produto, string filial)
        {
            string output = "";
            using (SqlCommand comando = conecxao.CreateCommand())
            {

                string query = String.Format(" select B1_REVATU " +
                                              " from SB1010 " +
                                              " WHERE B1_COD = '{0}' " +
                                              " AND B1_FILIAL={1} " +
                                              " AND D_E_L_E_T_ = '' ", produto, filial);
                comando.CommandText = query;
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        output = Convert.ToString(leitor["B1_REVATU"]);
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Obtem o rendimento de uma fórmula e produto
        /// </summary>
        /// <param name="produto">The produto.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="filial">The filial.</param>
        /// <returns></returns>
        public double GetRendimentoLast(string produto, string filial)
        {

            string formula = LastFormula(produto, filial);
            double output = 0;
            using (SqlCommand comando = conecxao.CreateCommand())
            {
                string query = String.Format("select ISNULL(Z01_REND,0) as Z01_REND" +
                                               " from Z01010 " +
                                               " WHERE Z01_COD = '{0}' " +
                                               " AND Z01_FORM = '{1}'" +
                                               " AND Z01_FILIAL={2} " +
                                               " AND D_E_L_E_T_ = ''", produto, formula, filial);
                comando.CommandText = query;

                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        output = Convert.ToDouble(leitor["Z01_REND"]);
                    }

                }
            }

            return output == 0 ? 100 : output;
        }


    }
}
