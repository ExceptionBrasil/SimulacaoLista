using ColorobbiaPlataform.Areas.Admin.Filters;
using DAL.Cadastros.Administracao.Menus;
using DAL.DAOs.Cadastros.Produtos;
using DAL.DAOs.Empresas;
using DAL.DAOs.Formulas;
using DAL.DAOs.SimulacaoLista;
using Factorys.Cadastros.Produto;
using Factorys.Simulacao;
using Factorys.Tools;
using Modelos.Admin;
using Modelos.Admin.Acessos;
using Modelos.Cadastros.Produtos;
using Modelos.Formulas;
using Modelos.SimulacaoLista;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.SimulacaoLista.Controllers
{
    /// <summary>
    /// Controle da Simulação 
    /// </summary>
    [FrontEndAutorize]
    public class SimuladorController : Controller
    {
        private readonly EstruturaProdutoDAO estruturaDAO;
        private readonly FiliaisDAO filiaisDAO;
        private readonly ProdutoDAO produtoDAO;
        private readonly RoleDAO roleDAO;
        private readonly PermissoesDaSimulacaoDAO permissaoDAO;

        public SimuladorController(EstruturaProdutoDAO sDAO,
                                    FiliaisDAO fDAO,
                                    ProdutoDAO pDAO,
                                    RoleDAO rDAO,
                                    PermissoesDaSimulacaoDAO permDAO)
        {
            this.estruturaDAO = sDAO;
            this.filiaisDAO = fDAO;
            this.produtoDAO = pDAO;
            this.roleDAO = rDAO;
            this.permissaoDAO = permDAO;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Usuario usuario = (Usuario)Session["usuario"];
            Role role = roleDAO.GetById((int)Session["role"]);
            var permissao = permissaoDAO.GetByRole(role);


            if (permissao.FazAprovacao)
            {
                ViewBag.Aprovados = produtoDAO.ListAproved();
                return View("Aprovador", produtoDAO.ListNonAproved());
            }

            var produtosSimulados = produtoDAO.ListByUser(usuario);
            return View(produtosSimulados);
        }

        /// <summary>
        /// Aprovadors the specified produtos simulados.
        /// </summary>
        /// <param name="produtosSimulados">The produtos simulados.</param>
        /// <returns></returns>
        public ActionResult Aprovador(IList<Produto> produtosSimulados)
        {
            ViewBag.Aprovados = produtoDAO.ListAproved();
            return View();
        }

        /// <summary>
        /// Aprova pelo ID
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Aprovar(int id)
        {
            produtoDAO.AprovarById(id, (Usuario)Session["usuario"]);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// reprova pelo ID
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Reprovar(int id)
        {
            produtoDAO.ReprovarById(id, (Usuario)Session["usuario"]);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Formulário de Exclusão
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Excluir(int id)
        {
            Produto produto = produtoDAO.GetById(id);
            var modelView = ProdutoFactory.BuildModelView(produto);
            ViewBag.UsuarioDeAprovacao = produto.UsuarioDeAprovacao;
            return View(modelView);
        }

        /// <summary>
        /// exclusão de uma simulação
        /// </summary>
        /// <param name="modelView">The model view.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Excluir(ProdutoModelView modelView)
        {
            produtoDAO.Delete(produtoDAO.GetById(modelView.Id));
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Retona a View com base no IdDaSimulacao
        /// </summary>
        /// <param name="idSimulacao">The identifier simulacao.</param>
        /// <param name="geraPdf">if set to <c>true</c> [gera PDF].</param>
        /// <returns></returns>
        public ActionResult Vizualizar(int idSimulacao, bool geraPdf)
        {
            var produto = produtoDAO.GetById(idSimulacao);

            ViewBag.Produto = produto;

            ViewBag.FormulaBase = produto.Formula;
            ViewBag.RendimentoEscolhido = produto.Rendimento;

            Usuario usuario = (Usuario)Session["usuario"];
            Role role = roleDAO.GetById((int)Session["role"]);

            ///Permissões
            PermissoesDaSimulacao permissoes = permissaoDAO.GetByRole(role);
            ViewBag.Permissoes = permissoes;

            if (!permissoes.DiscoverColunasDeCusto)
            {
                ViewBag.HiddenCol = "hidden";
            }
            else
            {
                ViewBag.HiddenCol = "";
            }


            //Base
            ViewBag.EstruturaBase = (from estrutura in produto.Estrutura
                                     where estrutura.Nivel <= permissoes.NivelMaximo
                                     select estrutura).ToList();
            //Gera Pdf
            if (geraPdf)
            {
                var viewPdf = new ViewAsPdf()
                {
                    ViewName = "Vizualizar",
                    IsGrayScale = true,
                    Model = produto.Estrutura

                };
                return viewPdf;
            }
            return View(produto.Estrutura);
        }

        /// <summary>
        /// Tela de  parâmetros novos produtos 
        /// </summary>
        /// <returns>
        /// ProdutoModelView
        /// </returns>
        public ActionResult NovoProduto()
        {
            ViewBag.Filial = new SelectList(filiaisDAO.Get(),
                                         "codigo",
                                         "descricaoResumida");
            ProdutoModelView modelView = new ProdutoModelView();
            return View(modelView);
        }

        /// <summary>
        /// Formulário de criação de novos peroduto
        /// </summary>
        /// <param name="produto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NovoProdutoForm(ProdutoModelView produto)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Filial = new SelectList(filiaisDAO.Get(),
                                         "codigo",
                                         "descricaoResumida");
                return View("NovoProduto", produto);
            }


            ViewBag.Filial = produto.Filial;
            ViewBag.ProdutoNovo = produtoDAO.GenerateByFramilia(produto);
            IList<EstruturaProduto> listEstruturaModelView = new List<EstruturaProduto>();

            return View(listEstruturaModelView);
        }

        /// <summary>
        /// Faz o salvamento de uma simulação
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SalvarSimulacao(string json,string nome)
        {
            //Convert a string em obj Json 
            JsonSimulacao jsonSimulacao = JsonConvert.DeserializeObject<JsonSimulacao>(json);

            if (jsonSimulacao.CustoTotalDoProduto == 0)
            {
                return Json(new { success = false, menssage = "Não é possível salvar produto com Custo Total Zerado." });
            }

            //Chama a factory para monta um produto 

            Produto produto = SimulacaoFactory.MakeProdutoByJson(jsonSimulacao);


            produto.DataDeCriacao = DateTime.Now;
            produto.UsuarioDeCriacao = (Usuario)Session["usuario"];
            produto.NomeDaSimulacao = nome;

            produtoDAO.Save(produto);

            return Json(new { success = true, menssage = "Simulacao Salva com sucesso!" });
        }

        /// <summary>
        /// Faz a simulação da Lista 
        /// </summary>
        /// <returns></returns>
        public ActionResult Parametros()
        {

            ViewBag.Filial = new SelectList(filiaisDAO.Get(),
                                            "codigo",
                                            "descricaoResumida");
            return View();
        }

        /// <summary>
        /// Gera a Simulção em Tela
        /// </summary>
        /// <param name="parametros">The parametros.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Simulacao(ParametrosDaSimulacao parametros)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);  // HTTP 1.1.
            Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.

            if (!SimulacaoValidate(parametros))
            {
                ViewBag.Filial = new SelectList(filiaisDAO.Get(),
                                          "codigo",
                                          "descricaoResumida");
                return View("Parametros", parametros);
            }
            ///Permissões
            Usuario usuario = (Usuario)Session["usuario"];
            Role role = roleDAO.GetById((int)Session["role"]);
            PermissoesDaSimulacao permissoes = permissaoDAO.GetByRole(role);

            /* Produto Simulacao*/
            Produto produtoSimulacao = await produtoDAO.GetByCodigoAsync(parametros.CodigoDoProduto.Substring(0, 15),
                                                                parametros.Filial,
                                                                parametros.FormulaSimulacao,
                                                                permissoes.TipoDeCalculo.Id);
            ViewBag.ProdutoSimulacao = produtoSimulacao;

            /*Produto Base*/
            Produto produtoBase = await produtoDAO.GetByCodigoAsync(parametros.CodigoDoProduto.Substring(0, 15),
                                                                parametros.Filial,
                                                                parametros.FormulaBase,
                                                                permissoes.TipoDeCalculo.Id);
            ViewBag.ProdutoBase = produtoBase;


            //
            ViewBag.FormulaSimulacao = parametros.FormulaSimulacao ?? produtoSimulacao.Formula;
            ViewBag.FormulaBase = parametros.FormulaBase ?? produtoBase.Formula;

            ViewBag.RendimentoEscolhido = produtoDAO.GetRendimentoLast(parametros.CodigoDoProduto,
                                                                        parametros.Filial);

            //Permissões
            ViewBag.Permissoes = permissoes;

            if (!permissoes.DiscoverColunasDeCusto)
            {
                ViewBag.HiddenCol = "hidden";
            }
            else
            {
                ViewBag.HiddenCol = "";
            }


            //Simulação
            var EstruturaDoProduto = (from estrutura in produtoSimulacao.Estrutura
                                      where estrutura.Nivel <= permissoes.NivelMaximo
                                      select estrutura).ToList();

            //Base
            ViewBag.EstruturaBase = (from estrutura in produtoBase.Estrutura
                                     where estrutura.Nivel <= permissoes.NivelMaximo
                                     select estrutura).ToList();


            return View(EstruturaDoProduto);
        }

        /// <summary>
        /// Faz as validações do ModelState
        /// </summary>
        /// <param name="parametros">The parametros.</param>
        /// <returns></returns>
        private bool SimulacaoValidate(ParametrosDaSimulacao parametros)
        {
            if (!ModelState.IsValid)
            {

                return false;
            }
            if (parametros.FormulaBase != null && parametros.FormulaSimulacao != null)
            {
                var formulaBase = produtoDAO.GetFormula(parametros.CodigoDoProduto.Substring(0, 15),
                                                     parametros.Filial,
                                                     parametros.FormulaBase);

                var formulaSimulacao = produtoDAO.GetFormula(parametros.CodigoDoProduto.Substring(0, 15),
                                                     parametros.Filial,
                                                     parametros.FormulaSimulacao);

                if (formulaBase.Count == 0)
                {
                    ModelState.AddModelError("FORMULA", "Essa fórmula BASE não foi encontrada.");
                    return false;
                }
                if (formulaSimulacao.Count == 0)
                {
                    ModelState.AddModelError("FORMULA", "Essa fórmula SIMULACAO não foi encontrada.");
                    return false;
                }
            }
            return true;

        }


    }
}
