using ColorobbiaPlataform.Areas.Admin.Filters;
using DAL.Cadastros.Administracao.Menus;
using DAL.DAOs.SimulacaoLista;
using Factorys.Simulacao;
using Modelos.SimulacaoLista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.SimulacaoLista.Controllers
{
    [FrontEndAutorize]
    public class SimuladorPermissoesController : Controller
    {
        private readonly PermissoesDaSimulacaoDAO permissaoDAO;
        private readonly RoleDAO roleDAO;
        private readonly TipoDeCalculosDAO tipoDeCalculo;

        public SimuladorPermissoesController(PermissoesDaSimulacaoDAO permiDAO,
            RoleDAO roleDAO,
            TipoDeCalculosDAO tipoCalc)
        {
            this.permissaoDAO = permiDAO;
            this.roleDAO = roleDAO;
            this.tipoDeCalculo = tipoCalc;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = permissaoDAO.List();
            return View(model);
        }


        /// <summary>
        /// Formulario de Inclusao
        /// </summary>
        /// <returns></returns>
        public ActionResult Incluir()
        {
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
            ViewBag.TipoDeCalculo = tipoDeCalculo.List(); 
            return View();
        }       

        /// <summary>
        /// Grava a Inclusão
        /// </summary>
        /// <param name="modelView">The model view.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(PermissoesDaSimulacaoModelView modelView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
                ViewBag.TipoDeCalculo = tipoDeCalculo.List();
                return View();
            }

            var model = PermissoesFactory.BuildModel(modelView);
            model.Role = roleDAO.GetById(modelView.Role);
            model.DataDeCriacao = DateTime.Now;
            model.TipoDeCalculo = tipoDeCalculo.GetById(modelView.TipoDeCalculo);

            permissaoDAO.Add(model);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Formulário de Alteração
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Alterar(int id)
        {

            var model = permissaoDAO.GetById(id);
            var modelView = PermissoesFactory.BuildModelView(model);
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description", modelView.Role.ToString());
            ViewBag.TipoDeCalculo = tipoDeCalculo.List();            
            return View(modelView);
        }



        /// <summary>
        /// Grava a alteração das permissoes
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar(PermissoesDaSimulacaoModelView modelView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
                return View();
            }

            var model = PermissoesFactory.BuildModel(modelView);
            model.Role = roleDAO.GetById(modelView.Role);
            model.TipoDeCalculo = tipoDeCalculo.GetById(modelView.TipoDeCalculo);

            permissaoDAO.Update(model);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// formulário de exclusão
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Excluir(int id)
        {

            var modelView = PermissoesFactory.BuildModelView(permissaoDAO.GetById(id));
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description", modelView.Role);
            ViewBag.TipoDeCalculo = tipoDeCalculo.List();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Excluir(PermissoesDaSimulacaoModelView modelView)
        {
            permissaoDAO.Delete(PermissoesFactory.BuildModel(modelView));
            return RedirectToAction("Index");
        }

    }
}
