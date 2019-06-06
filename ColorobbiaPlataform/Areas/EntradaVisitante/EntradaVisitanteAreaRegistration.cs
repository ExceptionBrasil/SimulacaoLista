using System.Web.Mvc;

namespace ColorobbiaPlataform.Areas.EntradaVisitante
{
    public class EntradaVisitanteAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EntradaVisitante";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EntradaVisitante_default",
                "EntradaVisitante/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}