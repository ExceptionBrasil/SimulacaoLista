using Modelos.Cadastros.Produtos;
using Modelos.Formulas;
using Modelos.SimulacaoLista;
using SessionControl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys.Simulacao
{
    public static class SimulacaoFactory
    {
        private static SqlConnection conecxao = SqlManagement.GetConnection();

        private static IList<EstruturaProduto> MakeEstruturaByArray(string[,] array, string filial, string grupo)
        {
            double percentualFurmula;
            double custoMateriaPrima;

            IList<EstruturaProduto> estrutura = new List<EstruturaProduto>();

            for (int i = 1; i < array.GetLength(0); i++)
            {
                try
                {
                    percentualFurmula = Convert.ToDouble(array[i, 6].Replace('.', ','));
                    custoMateriaPrima = Convert.ToDouble(array[i, 7].Replace('.', ','));
                }               
                catch(Exception e)
                {
                    percentualFurmula = 0;
                    custoMateriaPrima = 0;
                    Console.WriteLine(e.Message);
                }
               
                    //so alimenta se tiver preechido o percentual da fórmula
                    if (percentualFurmula>0)
                    {
                        estrutura.Add(new EstruturaProduto()
                        {
                            Filial = filial,
                            Nivel = Convert.ToInt32(array[i, 1]),
                            Produto = array[i, 2],
                            Componente = array[i, 3],
                            Tipo = array[i, 4],
                            Unidade = array[i, 5],
                            PercentualFormula = percentualFurmula,
                            CustoMateriPrima = custoMateriaPrima,
                            //  CustoTotalComponente = Convert.ToDouble(array[i, 8]),
                            CentroCustoComponente = GetCentroCustoByCod(array[i, 3], filial),
                            CentroCustoProduto = GetCentroCustoByCod(array[i, 2], filial),
                            DescricaoComponente = GetDescricaoByCod(array[i, 3], filial),
                            DespesaOperacional = GetCustoOperacional(grupo, filial),
                            Rendimento = 0,
                            UltimoCustoMedio = 0,
                            // CustoMedioUnitario = 0
                        });
                    }
               
               
            }
            return estrutura;
        }


        /// <summary>
        /// Cria um produto utilizando um retorno json
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static Produto MakeProdutoByJson(JsonSimulacao json)
        {
            Produto produto = new Produto();
            produto.Estrutura = MakeEstruturaByArray(json.Estrutura, json.Filial, json.Familia);

            produto.CustoEmbalagem = json.CustoEmbalagem;
            produto.CustoEmbalagemPercent = json.CustoEmbalagemPercent;
            produto.CustoIndustrial = json.CustoIndustrial;
            produto.CustoOperacional = json.CustoOperacional;
            produto.CustoTotalDoProduto = json.CustoTotalDoProduto;
            produto.CustoTotalDoProdutoMargem = json.CustoTotalDoProdutoMargem;
            produto.Descricao = json.Descricao;
            produto.DescricaoCCusto = json.DescricaoCCusto;
            produto.DespesasOperacionais = json.DespesasOperacionais;            
            produto.Familia = json.Familia;
            produto.Filial = json.Filial;
            produto.Formula = json.Formula;
            produto.Id = json.Id;
            produto.MargemLucro = json.MargemLucro;

            //produto.DespesasOperacionaisCalculada = json.DespesasOperacionaisCalculada;
            //produto.PrecoBase = json.PrecoBase;
            //produto.PrecoBaseIcm12 = json.PrecoBaseIcm12;
            //produto.PrecoBaseIcm18 = json.PrecoBaseIcm18;
            //produto.PrecoBaseIcm7 = json.PrecoBaseIcm7;
            produto.Rendimento = json.Rendimento;
            produto.CentroCusto = json.CentroCusto;
            produto.Codigo = json.Codigo;

            return produto;
        }

        /// <summary>
        /// Retorna a Descrição de um Produto cadastrado no SB1
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="filial"></param>
        /// <returns></returns>
        private static string GetDescricaoByCod(string codigo, string filial)
        {
            using (SqlCommand command = conecxao.CreateCommand())
            {
                string query = String.Format("SELECT B1_DESC " +
                    " FROM SB1010 " +
                    " WHERE " +
                    " D_E_L_E_T_='' " +
                    " AND B1_FILIAL='{0}' " +
                    " AND B1_COD='{1}'", filial, codigo);
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = query;

                using (SqlDataReader leitor = command.ExecuteReader())
                {
                    leitor.Read();
                    if (leitor.HasRows)
                    {
                        return leitor["B1_DESC"].ToString();
                    }
                    return "";
                }
            }
        }

        /// <summary>
        /// retorna o Despesa Operacional de um grupo de Produto
        /// </summary>
        /// <param name="grupo"></param>
        /// <param name="filial"></param>
        /// <returns></returns>
        private static double GetCustoOperacional(string grupo, string filial)
        {
            using (SqlCommand command = conecxao.CreateCommand())
            {
                string query = String.Format("SELECT ISNULL(Z07_CUSTO1,0) as Z07_CUSTO1 " +
                    " FROM Z07010 " +
                    " WHERE " +
                    " Z07_GRUPO='{0}' " +
                    " AND D_E_L_E_T_='' " +
                    " and Z07_FILIAL='{1}'", grupo, filial);
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = query;

                using (SqlDataReader leitor = command.ExecuteReader())
                {
                    leitor.Read();
                    if (leitor.HasRows)
                    {
                        return Convert.ToDouble(leitor["Z07_CUSTO1"]);
                    }
                    return 0;
                }
            }
        }


        /// <summary>
        /// Retorna o Centro de Custo de um Produto cadastrado no SB1
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="filial"></param>
        /// <returns></returns>
        private static string GetCentroCustoByCod(string codigo, string filial)
        {
            using (SqlCommand command = conecxao.CreateCommand())
            {
                string query = String.Format("SELECT B1_CC " +
                    " FROM SB1010 " +
                    " WHERE " +
                    " D_E_L_E_T_='' " +
                    " AND B1_FILIAL='{0}' " +
                    " AND B1_COD='{1}'", filial, codigo);
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = query;

                using (SqlDataReader leitor = command.ExecuteReader())
                {
                    leitor.Read();
                    if (leitor.HasRows)
                    {
                        return leitor["B1_CC"].ToString();
                    }
                    return "";
                }
            }
        }
    }
}
