using ColorobbiaPlataform.Areas.Admin.Filters;
using ControleDeArquivos;
using DAL.DAOs.Cadastros.Visitantes;
using Modelos.Cadastros.Visitantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace ColorobbiaPlataform.Areas.EntradaVisitante.Controllers
{
    /// <summary>
    /// Controller que gerencia as requisições relacionadas aos visitantes
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [FrontEndAutorize]
    public class VisitanteController : Controller
    {
        private VisitanteDAO visitanteDAO;
        private const int itensPorPagina = 50;

        public VisitanteController(VisitanteDAO v)
        {
            this.visitanteDAO = v;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index(int? pagina)
        {
            int paginaAtual = pagina ?? 1;
            IList<Visitante> paginaDeVisitante = await visitanteDAO.List(paginaAtual, itensPorPagina);            
            
            ViewBag.paginaDeVisitante = new StaticPagedList<Visitante>(paginaDeVisitante, 
                                                paginaAtual, 
                                                itensPorPagina, 
                                                visitanteDAO.Count());

            return View(paginaDeVisitante);
        }

        /// <summary>
        /// Visualiza um visitante
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Visualizar(int id)
        {
            Visitante visitante = visitanteDAO.GetById(id);
            ViewBag.ImagemB64 = visitante.FotoBase64;
            return View(visitante); 
        }

        /// <summary>
        /// Formulário de alteração de Visitante
        /// </summary>
        /// <returns></returns>
        public ActionResult Alterar(int id)
        {
            Visitante vis = visitanteDAO.GetById(id);
            return View(vis);
        }

        /// <summary>
        /// Alteração do visitante
        /// </summary>
        /// <param name="vis">The vis.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar(Visitante vis)
        {
            if (vis == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return View(vis);
            }

            IList<Visitante> visitante = visitanteDAO.GetAllSimilarsByDocumento(vis);

            if (visitante.Count>0)
            {
                ModelState.AddModelError("JAEXISTE", "Já existe um visitante com esse documento");
                return View(vis);
            }

            visitanteDAO.Alter(vis);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Blocks the visitante.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public ActionResult BlockVisitante(int id)
        {
            Visitante vis = visitanteDAO.GetById(id);
            return View(vis);
        }


        /// <summary>
        /// Blocks the visitante.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Block(int id)
        {
            Visitante vis = visitanteDAO.GetById(id);

            vis.Bloqueado = !vis.Bloqueado;
            visitanteDAO.Alter(vis);

            string redirTo = Url.Action("Index", "Visitante", new { Area = "EntradaVisitante" });

            return Json(new {success=true, mensage ="Visitante Alterado",redirect = redirTo });
          
        }



        /// <summary>
        /// Retorna um Json com a lista de visitantes que contenham o nome solicitado
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public JsonResult GetByName(string name)
        {
            var visitantes = visitanteDAO.GetByNome(name);
            return Json(new { success = true, visitantes });
        }


        /// <summary>
        /// Faz a exclusão de um visitante
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Excluir(int Id)
        {
            var visitante = visitanteDAO.GetById(Id);
            visitanteDAO.Delete(visitante);
            return RedirectToAction("Index");
        }

    }
}