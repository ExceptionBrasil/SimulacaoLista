using DAL.DAOs.Admin;
using Factorys;
using Modelos.Admin;
using SessionControl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private UsuarioDAO usuarioDAO;

        public LoginController(UsuarioDAO u)
        {
            this.usuarioDAO = u;
        }

        // GET: Admin/Login
        public ActionResult Index()
        {

            
            ViewBag.IsTest = NHibernateFactory.Ambiente
                            .ToUpper().Contains("PRODUCAO");

            Session["usuarioNome"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult ValidaLogin(Login login)
        {

            Usuario usuario = usuarioDAO.GetByLogin(login.UserName);

            if (usuario == null)
            {
                ModelState.AddModelError("Login", "Login inválido ou não encontrado");
                return View("Index", login);
            }


            if (!ModelState.IsValid)
            {
                return View("Index", login);
            }

            if (! PasswordHash.Verific(usuario.Password, login.Password))
            {
                ModelState.AddModelError("Login", "Senha inválida");
                return View("Index", login);
            }

            Session["usuario"] = usuario;
            Session["usuarioNome"] = usuario.Nome;
            Session["role"] = usuario.Role.Id;
            Session["admin"] = usuario.IsAdmin;
            Session.Timeout = 525600;

            return RedirectToAction("Index", "Home", new { area = "" });
               
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

    }
}