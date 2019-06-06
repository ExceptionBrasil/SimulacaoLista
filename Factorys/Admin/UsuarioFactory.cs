using Modelos.Admin;
using Modelos.Admin.Acessos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys.Admin
{
    public static class UsuarioFactory
    {
        /// <summary>
        /// Cria um usuário com base em uma UsuárioModelView
        /// </summary>
        /// <param name="modelView">The model view.</param>
        /// <returns></returns>
        public static Usuario BuildModel(UsuarioModelView modelView)
        {
            Usuario user = new Usuario()
            {
                Ativo = modelView.Ativo,
                DataCriacao = modelView.DataCriacao,
                Email = modelView.Email,
                Id = modelView.Id,
                IsAdmin = modelView.IsAdmin,
                LastAcess = modelView.LastAcess,
                Login = modelView.Login,
                Nome = modelView.Nome,
                Password = modelView.Password
            };

            return user;
        }

        /// <summary>
        /// Cria uma lista usuário com base em uma lista de UsuárioModelView
        /// </summary>
        /// <param name="modelViewList">The model view list.</param>
        /// <returns></returns>
        public static IList<Usuario> BuildModelList(IList<UsuarioModelView> modelViewList)
        {
            IList<Usuario> usuarios = new List<Usuario>();
            foreach (var item in modelViewList)
            {
                usuarios.Add(UsuarioFactory.BuildModel(item));
            }

            return usuarios;
        }

        /// <summary>
        /// Cria um usuário model view com base em uma model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static UsuarioModelView BuildModelView (Usuario model)
        {
            UsuarioModelView modelView = new UsuarioModelView()
            {
                Ativo = model.Ativo,
                DataCriacao = model.DataCriacao,
                Email = model.Email,
                Id = model.Id,
                IsAdmin = model.IsAdmin,
                LastAcess = model.LastAcess,
                Login = model.Login,
                Nome = model.Nome,
                Password = model.Password                
            };

            return modelView;
        }

        /// <summary>
        /// Cria uma lista de ModelView com base em uma lista de Models
        /// </summary>
        /// <param name="modelList">The model list.</param>
        /// <returns></returns>
        public static IList<UsuarioModelView> BuildModelViewList(IList<Usuario> modelList)
        {
            IList<UsuarioModelView> modelViewList = new List<UsuarioModelView>();

            foreach (var item in modelList)
            {
                modelViewList.Add(UsuarioFactory.BuildModelView(item));
            }
            return modelViewList;
        }
    }
}
