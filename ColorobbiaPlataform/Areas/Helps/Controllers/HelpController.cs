using ColorobbiaPlataform.Areas.Admin.Filters;
using DAL.Cadastros.Administracao.Menus;
using DAL.DAOs.Helps;
using Factorys.Helps;
using Modelos.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.Helps.Controllers
{
    [FrontEndAutorize]
    public class HelpController : Controller
    {
        private HelpDAO helpDAO;
        private RoleDAO roleDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpController"/> class.
        /// </summary>
        /// <param name="h">The h.</param>
        public HelpController(HelpDAO h,RoleDAO r)
        {
            this.helpDAO = h;
            this.roleDAO = r;
        }

        // GET: Helps/Home
        public ActionResult Index()
        {
            var helps = helpDAO.ListDescend();
            return View(helps);
        }

        /// <summary>
        /// Formulário de Inclusão
        /// </summary>
        /// <returns></returns>
        public ActionResult Incluir()
        {
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
            return View();
        }


        /// <summary>
        /// Incluirs the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Incluir(HelpView view)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
                return View(view);
            }

            Help help = HelpFactory.BuildModel(view);
            help.Role = roleDAO.GetById(view.Role);
            helpDAO.Add(help);

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Formulário de Alteração 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Alterar(int id)
        {
            var help = helpDAO.GetById(id);
            var modelView = HelpFactory.BuildModelView(help);
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description",help.Role.Id);
            return View(modelView);
        }

        /// <summary>
        /// Faz a alteração de uma publicação
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Alterar(HelpView view)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description", view.Role);
                return View(view);
            }

            Help help = HelpFactory.BuildModel(view);
            help.Role = roleDAO.GetById(view.Role);

            helpDAO.Alter(help);

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Publica um aviso
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
       
        public ActionResult Publicar(int id)
        {
            var help = helpDAO.GetById(id);
            help.DataPublicacao = DateTime.Now;
            helpDAO.Alter(help);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Excluirs the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Excluir(int id)
        {
            var help = helpDAO.GetById(id);
            help.DataPublicacao = DateTime.Now;
            helpDAO.Delete(help);
            return RedirectToAction("Index");
        }
    }
}