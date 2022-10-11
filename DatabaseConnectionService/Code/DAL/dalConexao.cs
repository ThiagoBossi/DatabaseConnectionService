using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionService.Code.DAL
{
    public class dalConexao
    {
        public static SqlConnection cnn { get; set; }
        
        public  bool Conectar()
        {
            try
            {
                cnn = new SqlConnection();
                cnn.ConnectionString = "Data Source=127.0.0.1;Initial Catalog=Siscom;Integrated Security=SSPI;Persist Security Info=True";
                cnn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Desconectar()
        {
            if (cnn.State == ConnectionState.Open)
            {
                cnn.Close();
                return true;
            }

            return false;
        }
    }
}
