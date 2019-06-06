using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SessionControl
{
    public static class NHibernateFactory
    {
        private static ISessionFactory fatory = BuildSession();
        public static string Ambiente { get; private set; }

        private static ISessionFactory BuildSession()
        {
            Configuration cfg = new Configuration();
            cfg.Configure();
            
            Ambiente = cfg.Properties["connection.connection_string_name"];

            return Fluently.Configure(cfg)
              .Mappings(x =>
                     x.FluentMappings.AddFromAssembly(
                Assembly.Load("Modelos")
                )
              ).BuildSessionFactory();

        }

        public static ISession OpenSession()
        {
            return fatory.OpenSession();
        }
    }
}
