using ColorobbiaPlataform.Areas.Admin.Filters;
using ControleDeArquivos;
using DAL.DAOs.Cadastros.Visitantes;
using Factorys.Cadastros.Visitantes;
using Factorys.Tools;
using Modelos.Admin;
using Modelos.Cadastros.Visitantes;
using Modelos.Relatorios.Parametros;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace ColorobbiaPlataform.Areas.EntradaVisitante.Controllers
{
    [FrontEndAutorize]
    public class ControleDeAcessoController : Controller
    {
        private ControleDeAcessoDAO acessoDAO;
        private VisitanteDAO visitanteDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControleDeAcessoController"/> class.
        /// </summary>
        /// <param name="a">a.</param>
        public ControleDeAcessoController(ControleDeAcessoDAO a, VisitanteDAO v)
        {
            this.acessoDAO = a;
            this.visitanteDAO = v;
        }


        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = acessoDAO.AcessosSemDataDeSaida();
            return View(model);
        }

        /// <summary>
        /// Ultimos acessos.
        /// </summary>
        /// <returns></returns>
        public ActionResult UltimosAcessos()
        {

            var model = acessoDAO.List(100);
            return View("Index", model);
        }

        /// <summary>
        /// Faz a inclusão de um acesso
        /// </summary>
        /// <returns></returns>
        public ActionResult Incluir()
        {
            return View();
        }

        /// <summary>
        /// Inclui um novo acesso para o Vistante
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(AcessoModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Transforma em visitante a modelView dos acessos
            Visitante visitante = VisitanteFactory.BuildModel(model);


            //Cria um controle de Acesso com base na modelView
            ControleDeAcesso acesso = ControleDeAcessoFactory.BuildModel(model);
            acesso.DataEntrada = DateTime.Now;

            //Adiciona o visitante no controle de acesso
            acesso.Visitante = visitante;

            //Grava quem fez o registro
            Usuario user = (Usuario)Session["usuario"];
            acesso.Usuario = user;
            acesso.DataInclusao = DateTime.Now;

            //Verifica se o visitante já está na base antes de persisti-lo          
            Visitante visita = visitanteDAO.GetByDocumento(visitante.Documento);

            if (visita == null)
            {

                visitante.Nome = visitante.Nome.ToUpper();
                visitante.Empresa = visitante.Empresa.ToUpper();
                visitante.FotoId = visitante.FotoBase64.GetHashMD5();
                //Persiste o Visitante
                visitanteDAO.Add(visitante);

                //Persiste o acesso
                acessoDAO.Add(acesso);

                return RedirectToAction("Index");
            }

            //verifica se o visitante não está bloqueado antes de persistir o acesso 
            if (visita.Bloqueado)
            {
                ModelState.AddModelError("BLQ", "Visitante bloqueado!");
                return View(model);
            }

            //Altera o visitante com a nova Foto        
            visita.FotoBase64 = model.Foto;
            visita.FotoId = model.Foto.GetHashMD5();


            visitanteDAO.Alter(visita);

            //Persiste o acesso
            acesso.Visitante = visita;
            acessoDAO.Add(acesso);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Formulário de alteração
        /// </summary>
        /// <returns></returns>
        public ActionResult Alterar(int id)
        {
            var model = acessoDAO.GetById(id);

            //Trasporte da foto
            ViewBag.ImagemB64 = model.Visitante.FotoBase64;

            var modelView = ControleDeAcessoFactory.BuildModelView(model);

            if (model.DataSaida == null)
            {
                return View(modelView);
            }
            return View("NaoAltera");
        }


        /// <summary>
        /// Faz a aletração de um acesso.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar(AcessoModelView modelView)
        {
            if (!ModelState.IsValid)
            {
                return View(modelView);
            }

            //Modificações do Acesso
            ControleDeAcesso acessoModificado = acessoDAO.GetById(modelView.Id);
            acessoModificado.Motivo = modelView.Motivo;
            acessoModificado.Visitado = modelView.Visitado;

            Visitante visitante = visitanteDAO.GetById(modelView.IdVisitante);
            visitante.FotoBase64 = modelView.Foto;
            visitante.FotoId = modelView.Foto.GetHashMD5();

            //persiste o visitante
            visitanteDAO.Alter(visitante);

            //Persiste o acesso
            acessoDAO.Alter(acessoModificado);

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Formulário de Exclusão
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Excluir(int id)
        {

            var model = acessoDAO.GetById(id);
            ViewBag.ImagemB64 = model.Visitante.FotoBase64;

            var modelView = ControleDeAcessoFactory.BuildModelView(model);
            return View(modelView);
        }

        [HttpPost]
        public ActionResult Excluir(AcessoModelView modelView)
        {
            ControleDeAcesso acesso = acessoDAO.GetById(modelView.Id);

            if (acesso.DataSaida == null)
            {
                acessoDAO.Delete(acesso);
                return RedirectToAction("Index");
            }
            return View("NaoExclui");

        }
        /// <summary>
        /// Retorna um Json do visitentejá tenha cadastro
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public JsonResult GetVisitor(string document)
        {
            var visitante = visitanteDAO.GetByDocumento(document);

            if (visitante != null)
            {
                if (visitante.Bloqueado)
                {
                    return Json(new { success = false, mensagem = "Visitante Bloqueado", block = true });
                }
                return Json(new { success = true, visitante, block = false });
            }
            return Json(new { success = false, mensagem = "Visitante não encontrado", block = true });
        }

        /// <summary>
        /// Formulário de alteração
        /// </summary>
        /// <returns></returns>
        public ActionResult GerarSaida(int id)
        {
            var model = acessoDAO.GetById(id);
            return View(model);
        }


        /// <summary>
        /// Faz a aletração de um acesso.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GerarSaida(ControleDeAcesso model)
        {

            var acesso = acessoDAO.GetById(model.Id);
            acesso.DataSaida = model.DataSaida;

            //Persiste o acesso
            acessoDAO.Alter(acesso);

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Relatorio de acessos 
        /// </summary>
        /// <returns></returns>
        public ActionResult RelAcessos()
        {
            return View(new RelAcessosParametros());
        }


        /// <summary>
        /// Exibe o relatório de acesso com base nos parametros informados
        /// </summary>
        /// <param name="parametros">The parametros.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RelAcessosResult(RelAcessosParametros parametros)
        {


            if (!ModelState.IsValid)
            {
                return View("RelAcessos", parametros);
            }

            IList<ControleDeAcesso> acessos = acessoDAO.Relatorio(parametros);
            var modelView = ControleDeAcessoFactory.BuildModelViewList(acessos);

            if (parametros.GerarPdf == true)
            {


                var novoRelatorio = new ViewAsPdf()
                {
                    ViewName = "RelAcessosResult",
                    IsGrayScale = true,
                    Model = modelView
                };

                return novoRelatorio;

            }
            else
            {
                return View(modelView);
            }


        }
    }
}