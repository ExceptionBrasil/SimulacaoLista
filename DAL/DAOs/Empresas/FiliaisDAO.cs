using Modelos.Empresas;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAOs.Empresas
{
    public class FiliaisDAO
    {
        private IList<Filial> filial;
        private SqlConnection conecxao;

        public FiliaisDAO(SqlConnection connection)
        {
            this.conecxao = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FiliaisDAO"/> class.
        /// </summary>
        public IList<Filial> Get()
        {

            using (SqlCommand comando = conecxao.CreateCommand())
            {
                comando.CommandText = " select M0_FILIAL,RTRIM(M0_CODFIL) AS M0_CODFIL," +
                                      " M0_NOMECOM,M0_CGC,M0_INSC " +
                                      " from SM001001 " +
                                      "ORDER BY M0_CODFIL ";

                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    filial = new List<Filial>();

                    while (leitor.Read())
                    {
                        filial.Add(new Filial()
                        {
                            Cnpj = Convert.ToString(leitor["M0_CGC"]),
                            Codigo = Convert.ToString(leitor["M0_CODFIL"]),
                            DescricaoResumida = Convert.ToString(leitor["M0_FILIAL"]),
                            Ie = Convert.ToString(leitor["M0_INSC"]),
                            RazaoSocial = Convert.ToString(leitor["M0_NOMECOM"])
                        });
                    }
                }
            }
            return filial;
        }



    }
}
