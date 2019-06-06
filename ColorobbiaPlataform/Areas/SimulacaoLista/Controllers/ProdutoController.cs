using ColorobbiaPlataform.Areas.Admin.Filters;
using DAL.DAOs.Cadastros.Produtos;
using DAL.DAOs.Empresas;
using Modelos.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.SimulacaoLista.Controllers
{   
    [FrontEndAutorize]
    public class ProdutoController : Controller
    {
        private ProdutoDAO produtoDAO;

        public ProdutoController(ProdutoDAO pDAO)
        {
            this.produtoDAO = pDAO;
        }

        // GET: SimulacaoLista/Produto
        public ActionResult Index()
        { 

            return View();
        }

        /// <summary>
        /// Busca um produto pelo código ou descrição, utilizando uma chave de pesquisa
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Retorna um Json contando uma lista de string
        /// <code> Json(new{List<string>})</code>
        /// </returns>
        public async Task<JsonResult> GetProdutoBySeachAsync(string search,string filial)
        {

            var listaProdutos = await produtoDAO.GetProdutoListAsync(search,filial);

            return Json(new { produtos = listaProdutos, success= true});
        }

  
        /// <summary>
        /// Retorna os dados estruturais de um componente isolado
        /// </summary>
        /// <param name="filial"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetInformationByCodigo(string filial,string codigo,string formula)
        {
            codigo = codigo.PadRight(15, ' ');
            EstruturaProduto estru = await produtoDAO.GetDadosDaEstruturaAsync(filial, codigo.Substring(0, 15));
            return Json(new { estrutura = estru, success = true });
        }

        /// <summary>
        /// Gets the formula by produto.
        /// </summary>
        /// <param name="produto">The produto.</param>
        /// <param name="filial">The filial.</param>
        /// <returns></returns>
        public JsonResult GetFormulaByProduto(string produto,string filial)
        {

            IList<string> codigoDaFormula = produtoDAO.GetFormula(produto
                                                                .PadRight(15,' ')
                                                                .Substring(0,15),filial);

            return Json(new {formulas= codigoDaFormula,success=true });
        }


        
    }
}