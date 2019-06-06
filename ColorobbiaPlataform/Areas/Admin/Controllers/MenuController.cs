using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ColorobbiaPlataform.Areas.Admin.Filters;
using DAL.Cadastros.Administracao.Menus;
using DAL.DAOs.Admin;
using Factorys.Admin;
using Modelos.Admin;
using Modelos.Admin.Acessos;

namespace ColorobbiaPlataform.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller para cadastrar e criar perfis de acesso
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [FrontEndAutorize]
    public class MenuController : Controller
    {   
        private MenuDAO menuDAO;
        private RoleDAO roleDAO;
        private UsuarioDAO usuarioDAO;

        public MenuController(MenuDAO m, RoleDAO r,UsuarioDAO u)
        {
            
            this.menuDAO = m;
            this.roleDAO = r;
            this.usuarioDAO = u;
        }

        /// <summary>
        /// View pricipal do controller
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index() => View();

        /// <summary>
        /// Index das roles 
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult IndexRoles()
        {
            var model = roleDAO.List();
            return View(model);
        }


        /// <summary>
        /// Formulário de Inclusão de Regras
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult IncluirRole()
        {
            return View();
        }


        /// <summary>
        /// Faz a inclusão de uma regra
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirRole(Role role)
        {
            if (role == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Erro", "Um ou mais campos estão preechidos de forma incorreta");
                return View(role);
            }

            roleDAO.Add(role);
            return RedirectToAction("IndexRoles");
        }


        /// <summary>
        /// Formulário de alteração de Role
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult AlterarRole(int id)
        {
            var model = roleDAO.GetById(id);
            return View(model);
        }


        /// <summary>
        /// Realiza a alteração de Uma Role 
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarRole(Role role)
        {

            if (role == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            roleDAO.Alter(role);
            return RedirectToAction("IndexRoles");
        }

        /// <summary>
        /// formulário de exclusão de uma role 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult ExcluirRole(int id)
        {
            var model = roleDAO.GetById(id);
            return View(model);
        }


        /// <summary>
        /// Faz a exclusão de uma role 
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            roleDAO.Delete(role);
            return RedirectToAction("IndexRoles");
        }



        /// <summary>
        /// Indexes dos Menus
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult IndexMenus()
        {
            var model = menuDAO.List();
            return View(model);
        }


        /// <summary>
        /// Formulário de inclusão de Menu
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult IncluirMenu()
        {
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
            return View();
        }


        /// <summary>
        /// Grava o menu 
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncluirMenu(MenuModelView model)
        {
            
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description",model.Role);
                return View(model);
            }
            var menu = MenuFactory.BuildModel(model);
            menu.Role = roleDAO.GetById(model.Role);

            menuDAO.Add(menu);
            return RedirectToAction("IndexMenus");
        }


        /// <summary>
        /// Formulário de alteração de Menu
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult AlterarMenu(int id)
        {
            Menu menu= menuDAO.GetById(id);
            var model = MenuFactory.BuildModelView(menu);

            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description", model.Role);
            return View(model);
        }

        /// <summary>
        /// Faz a alteração do Menu
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlterarMenu(MenuModelView menu)
        {

            if (menu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description", menu.Role);
                return View(menu);
            }

            
            var model = MenuFactory.BuildModel(menu);
            model.Role = roleDAO.GetById(menu.Role);
            menuDAO.Alter(model);

            return RedirectToAction("IndexMenus");
        }

        /// <summary>
        /// Formulário de alteração de Menu
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult ExcluirMenu(int id)
        {
            Menu menu= menuDAO.GetById(id);
            var model = MenuFactory.BuildModelView(menu);
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description",model.Role);
            return View(model);
        }

        /// <summary>
        /// Faz a alteração do Menu
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirMenu(MenuModelView menu)
        {

            if (menu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description",menu.Role);
                return View(menu);
            }


            var model = MenuFactory.BuildModel(menu);
            model.Role = roleDAO.GetById(menu.Role);
            menuDAO.Delete(model);

            return RedirectToAction("IndexMenus");
        }

        /// <summary>
        /// Obtem todos os menus que pertencem a uma certa localização, por exe.: Quero todos os menus que pertecem a Home
        /// Conforme a regra do usuário
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetByLocation(string Location)
        { 
            int roleId = (int)Session["role"];

            //recupera a session logada
            Usuario usuario = (Usuario)Session["usuario"];
            Role role = roleDAO.GetById(roleId);
            
            
            if (role != null)
            {
                IList<Menu> menus = menuDAO.RecoveryByLocation(Location, role.Value);
                

                //Se não há menus, retorna sucesso=falso
                if (menus.Count <= 0)
                {
                    return Json(new { success = false});
                }


                IList<MenuModelView> model= MenuFactory.BuildModelViewList(menus);

                //Ordena por grupo para montar os collaps corretamente
                var menu = model.OrderBy(x => x.Grupo);
                return Json(new { success = true,menu });
            }
            return Json(new { success = true, menu = "" });

        }
    }
}