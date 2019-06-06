using ColorobbiaPlataform.Areas.Admin.Filters;
using DAL.Cadastros.Administracao.Menus;
using DAL.DAOs.Admin;
using Factorys;
using Factorys.Admin;
using Modelos.Admin;
using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.Admin.Controllers
{
    [FrontEndAutorize]
    public class AccountController : Controller
    {
        private UsuarioDAO usuarioDAO;
        private RoleDAO roleDAO;

        public AccountController(UsuarioDAO user,RoleDAO rDAO)
        {
            this.usuarioDAO = user;
            this.roleDAO = rDAO;
        }

        /// <summary>
        /// Cadastro de Usuários
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var usuario = usuarioDAO.List();
            return View(usuario);
        }


        /// <summary>
        /// formulário de Inclusão
        /// </summary>
        /// <returns></returns>
        public ActionResult Incluir()
        {
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
            return View();
        }

        /// <summary>
        /// Formulário de troca de senha
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TrocarSenha(string novaSenha)
        {
            Usuario user =  (Usuario)Session["usuario"];
            Usuario usuario = usuarioDAO.GetById(user.Id);            
            usuario.Password = PasswordHash.Hash(novaSenha);
            usuarioDAO.Alter(usuario);

            return Json(new { success = true, menssage = "Senha alterada!" });
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(UsuarioModelView user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }


            user.Password = PasswordHash.Hash(user.Password);
            user.DataCriacao = DateTime.Now;

            Usuario usuario = UsuarioFactory.BuildModel(user);
            usuario.Role = roleDAO.GetById(user.Role);

            usuarioDAO.Add(usuario);
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Formulário de alteração 
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public ActionResult Alterar(int id)
        {            
            var model = usuarioDAO.GetById(id);

            var modelView = UsuarioFactory.BuildModelView(model);

            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description",model.Role);
            return View(modelView);
        }


        /// <summary>
        /// Realiza a alteração de um usuário
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar(UsuarioModelView user)
        {
           

            if (!ModelState.IsValid)
            {
                ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description", user.Role);
                return View(user);
            }

            user.Password = PasswordHash.Hash(user.Password);
            Usuario usuario = UsuarioFactory.BuildModel(user);
            usuario.Role = roleDAO.GetById(user.Role);

            usuarioDAO.Alter(usuario);


            return RedirectToAction("Index");
        }

        /// <summary>
        /// Formulário de exclusão 
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public ActionResult Inativar(int id)
        {
            
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
            var modelView = UsuarioFactory.BuildModelView(usuarioDAO.GetById(id));
            return View(modelView);
        }

        /// <summary>
        /// Faz a exclusão de um usuário
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inativar(UsuarioModelView user)
        {
            Usuario usuario = usuarioDAO.GetById(user.Id);            
            usuario.Ativo = !usuario.Ativo;            
            usuarioDAO.Alter(usuario);
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Formulário de Vizualização de Usuários 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult Vizualizar(int id)
        {
            ViewBag.Role = new SelectList(roleDAO.List(), "Id", "Description");
            var modelView = UsuarioFactory.BuildModelView(usuarioDAO.GetById(id));
            return View(modelView);
        }

    }
}