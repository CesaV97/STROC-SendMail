using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace STROC_SendMail
{
    internal class conexion
    {
        //SQLiteConnection con = new SQLiteConnection("Data Source=DBAmphenol.sqlite");  
        SqlConnection con = new SqlConnection("data source = 172.16.45.41; initial catalog = Compras; user id = serverMX01; password = Start001");

        //SqlConnection con = new SqlConnection("data source = LAPTOP-EDT30NN4\\LAPTOP; initial catalog = Entrenamiento; integrated security = true");
        public void conectar()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception)
            {

            }
        }

        public void desconectar()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        public string getDato(string consulta, SqlTransaction tran)
        {
            string resultado = "";
            try
            {
                if (tran == null)
                    conectar();
                //SQLiteCommand comando = con.CreateCommand();
                SqlCommand comando = con.CreateCommand();
                if (tran != null)
                    comando.Transaction = tran;
                comando.CommandText = consulta;
                resultado = comando.ExecuteScalar().ToString();
            }
            catch (Exception)
            {

            }
            finally
            {
                desconectar();
            }
            return resultado;
        }

        public DataTable getDatos(string consulta)
        {
            DataTable dt = new DataTable();
            try
            {
                conectar();

                //SQLiteDataAdapter sda = new SQLiteDataAdapter(consulta, con);
                SqlDataAdapter sda = new SqlDataAdapter(consulta, con);
                sda.Fill(dt);
                desconectar();

            }
            catch (Exception)
            {

            }
            return dt;

        }

        public SqlConnection getConection()
        {
            return con;
        }
    }
}
