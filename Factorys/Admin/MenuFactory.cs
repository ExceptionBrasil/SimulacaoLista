using Modelos.Admin.Acessos;
using System.Collections.Generic;

namespace Factorys.Admin
{
    /// <summary>
    /// Factory de construção de Modelos de Menus
    /// </summary>
    public static class MenuFactory
    {

        /// <summary>
        /// Constroi um modelo de menu com base em uma ModelView
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Menu BuildModel(MenuModelView model)
        {
            Menu menu = new Menu()
            {
                Action = model.Action,
                Area = model.Area,
                Controller = model.Controller,
                Descricao = model.Descricao,
                Grupo = model.Grupo,
                Id = model.Id,
                Location = model.Location,
                Icon = model.Icon
             
            };
            return menu;
        }

        /// <summary>
        /// Convert uma lista de Model Views em uma lista de Models
        /// </summary>
        /// <param name="modelList">The model list.</param>
        /// <returns></returns>
        public static IList<Menu> BuildModelList (IList<MenuModelView> modelList)
        {
            IList<Menu> menus = new List<Menu>();

            foreach(var menu in modelList)
            {

                menus.Add(BuildModel(menu));
            }

            return menus;

        }

        /// <summary>
        /// constroi uma Model View como base em  uma Model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static MenuModelView BuildModelView(Menu model)
        {
            MenuModelView modelView = new MenuModelView()
            {
                Role = model.Role.Id,
                Id = model.Id,
                Action = model.Action,
                Area = model.Area,
                Controller = model.Controller,
                Descricao = model.Descricao,
                Grupo = model.Grupo,
                Location = model.Location,
                Icon = model.Icon
            };

            return modelView;
        }

        /// <summary>
        /// Constroi uma lista de Menus com base em uma lista de Model Views
        /// </summary>
        /// <param name="modelList">The model list.</param>
        /// <returns></returns>
        public static IList<MenuModelView> BuildModelViewList(IList<Menu> modelList)
        {
            IList<MenuModelView> lista = new List<MenuModelView>();

            foreach (var menu in modelList)
            {
                lista.Add(BuildModelView(menu));
            }

            return lista;
        }
    }
}
