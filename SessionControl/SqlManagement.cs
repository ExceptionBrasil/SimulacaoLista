using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionControl
{
    public static class SqlManagement
    {       

        public  static SqlConnection GetConnection()
        {
            string connectionString = "Server=192.168.1.17;Database=PROTHEUS_PRODUCAO;User Id=totvs;Password=totvs#741";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

    }
}
