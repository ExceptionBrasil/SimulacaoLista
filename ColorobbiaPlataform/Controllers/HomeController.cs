using ColorobbiaPlataform.Areas.Admin.Filters;
using ColorobbiaPlataform.Helpers.Html;
using DAL.Cadastros.Administracao.Menus;
using DAL.DAOs.Helps;
using Modelos.Admin;
using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Controllers
{
    [FrontEndAutorize]
    public class HomeController : Controller
    {
        private HelpDAO helpDAO;
        private RoleDAO roleDAO;

        public HomeController(HelpDAO h,RoleDAO r)
        {
            this.helpDAO = h;
            this.roleDAO = r;
        }

        
        public ActionResult Index()
        {
            Usuario usuario = (Usuario)Session["usuario"];
            Role role = roleDAO.GetById((int)Session["role"]);

            ViewBag.Novidades= helpDAO.GetNews(role);
            return View(usuario);
        }
    }
}