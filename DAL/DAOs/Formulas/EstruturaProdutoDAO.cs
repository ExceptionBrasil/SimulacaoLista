using Modelos.Formulas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace DAL.DAOs.Formulas
{
    public class EstruturaProdutoDAO
    {
        private readonly ISession session;
        private readonly SqlConnection conecexao;
        public EstruturaProdutoDAO(ISession s, SqlConnection connection)
        {
            this.session = s;
            this.conecexao = connection;

        }


        /// <summary>
        /// Retorna a estrutura de produtos
        /// </summary>
        /// <param name="parametros">The parametros.</param>
        /// <returns></returns>
        public async Task<IList<EstruturaProduto>> GetEstruturaAsync(string produto, string filial, string formula,int tipoCalculo)
        {
            double custoMateriaPrima;
            IList<EstruturaProduto> estrutura = new List<EstruturaProduto>();

            using (SqlCommand comando = conecexao.CreateCommand())
            {

              
                comando.CommandText = "Usp_ListModel";
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@ProdutoInicial", produto);                
                comando.Parameters.AddWithValue("@Almoxarifado", "01");
                comando.Parameters.AddWithValue("@Filial", filial);                
             

                if (!String.IsNullOrEmpty(formula))
                {
                    comando.Parameters.AddWithValue("@RevisaoEscolhida", formula); 
                }

                using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                {

                    while (leitor.Read())
                    {
                        if (tipoCalculo == 1)
                        {
                             custoMateriaPrima = Convert.ToDouble(leitor["CusMP"]);
                        }
                        else
                        {
                             custoMateriaPrima = Convert.ToDouble(leitor["UltimoCustoMedio"]);
                        }
                        

                        estrutura.Add(new EstruturaProduto()
                        {
                            Id = Convert.ToInt32(leitor["Id"]),
                            Filial = Convert.ToString(leitor["Filial"]),
                            Componente = Convert.ToString(leitor["Componente"]),
                            DescricaoComponente = Convert.ToString(leitor["DescricaoComponente"]),
                            Tipo = Convert.ToString(leitor["TipoComponente"]),
                            Unidade = Convert.ToString(leitor["Unidade"]),
                            PercentualFormula = Convert.ToDouble(leitor["RendTeorico"]),
                            UltimoCustoMedio = Convert.ToDouble(leitor["UltimoCustoMedio"]),
                            Nivel = Convert.ToInt32(leitor["Nivel"]),
                            Produto = Convert.ToString(leitor["Produto"]),
                            CentroCustoComponente = Convert.ToString(leitor["CComponente"]),
                            CentroCustoProduto = Convert.ToString(leitor["CCProduto"]),
                            Rendimento = Convert.ToDouble(leitor["RendReal"]),
                            CustoMateriPrima = custoMateriaPrima,
                            DespesaOperacional = Convert.ToDouble(leitor["DespesaOP"])
                        });
                    }
                }
            }

            return estrutura;

        }
    }
}
