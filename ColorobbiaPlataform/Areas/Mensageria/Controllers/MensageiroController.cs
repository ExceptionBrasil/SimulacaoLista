using ColorobbiaPlataform.Areas.Admin.Filters;
using DAL.DAOs.Mensageria;
using Modelos.Mensageria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.Mensageria.Controllers
{
    [FrontEndAutorize]
    public class MensageiroController : Controller
    {
        private MensagensTruckDAO truckDAO;

        public MensageiroController(MensagensTruckDAO m)
        {
            this.truckDAO = m;
        }

        // GET: Mensageria/Mensageiro
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IncluirMensagemTruck (MensagensTruck mTruck)
        {
            if (!ModelState.IsValid)
            {
                return View(mTruck);
            }
            mTruck.DataCriacao = DateTime.Now;
            truckDAO.Add(mTruck);
            return RedirectToAction("Index");
        }

        
        
    }
}