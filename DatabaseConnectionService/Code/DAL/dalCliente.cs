using DatabaseConnectionService.Code.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionService.Code.DAL
{
    internal class dalCliente
    {
        public bool verificarCpfExistente(string cpf)
        {
            var retorno = false;

            var ssql = $"SELECT * FROM Clientes WHERE CPF='{cpf}'";
            using (var cmd = new SqlCommand(ssql, dalConexao.cnn))
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    retorno = true;
                }
                dr.Close();
            }

            return retorno;
        }

        public int receberNovoCodigo()
        {
            var retorno = 0;

            var ssql = "SELECT ISNULL(MAX(Codigo), 0) AS 'Codigo' FROM Clientes";

            using (var cmd = new SqlCommand(ssql, dalConexao.cnn))
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    retorno = Convert.ToInt32(dr["Codigo"].ToString()) + 1;
                }

                dr.Close();
            }

            return retorno;
        }

        public bool inserirRegistro(dtoCliente dto)
        {
            var codigo = receberNovoCodigo();

            var ssql = $"INSERT INTO Clientes (Codigo, Nome, Celular, CPF) VALUES (@codigo, @nome, @celular, @cpf)";
            using (var cmd = new SqlCommand(ssql, dalConexao.cnn))
            {
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@nome", dto.nome);
                cmd.Parameters.AddWithValue("@celular", dto.celular);
                cmd.Parameters.AddWithValue("@cpf", dto.cpf);

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
