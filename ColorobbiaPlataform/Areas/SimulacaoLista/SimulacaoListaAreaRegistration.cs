using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.SimulacaoLista
{
    public class SimulacaoListaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SimulacaoLista";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SimulacaoLista_default",
                "SimulacaoLista/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}