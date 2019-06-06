using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ColorobbiaPlataform.Helpers.Html
{
    /// <summary>
    /// Informações públicas 
    /// </summary>
    public static class InformationsHelper
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public static string Version { get
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fileV = FileVersionInfo.GetVersionInfo(assembly.Location);                 
                return fileV.FileVersion;
            }
        }
    }
}