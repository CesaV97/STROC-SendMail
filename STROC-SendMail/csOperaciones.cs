using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace STROC_SendMail
{
    internal class csOperaciones
    {
        string sSQL = "";
        string sSQL01 = "";
        conexion Cnx;
        SqlCommand oCmd;
        SqlDataAdapter oDA;
        DataTable DT;
        DataTable oDtLlenaDgv;
        //string constr = "data source = 172.16.40.37; initial catalog = Entrenamiento; user id = KE_Entrenamiento; password = Start010";

        public csOperaciones()
        {
            Cnx = new conexion();
            Cnx.conectar();
        }
        //Metodo comun para obtener datos
        private DataTable obtenerDatos(string consulta)
        {
            oCmd = Cnx.getConection().CreateCommand();
            oCmd.CommandText = consulta;
            oDA = new SqlDataAdapter(oCmd);
            DT = new DataTable();
            try
            {
                oDA.Fill(DT);
            }
            catch (Exception)
            {
                DT = null;
            }
            return DT;
        }
        //Actualizar contraseña
        public Int16 actualizarContraseña(String _contraseña, String _nomina)
        {
            if (Cnx.getConection().State == ConnectionState.Closed)
            {
                Cnx.getConection().Open();
            }
            //sSQL = "insert into tbTest values ('" + _nombre + "'," + _contador + ")";
            sSQL = "update tbusuarios Set contraseña = " + "'" + _contraseña + "'" + " where usuario = '" + _nomina + "'";
            try
            {
                oCmd = Cnx.getConection().CreateCommand();
                oCmd.CommandText = sSQL;
                oCmd.CommandTimeout = 0;
                oCmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /*Actualizar estatus (updateState) de requisicon en tbflujo parametros {Estatus:Int16, _usuario:Int16}
        Estatus 01 - Requisicion creada por usuario
        Estatus 02 - Requisicion aprobada por aprobador de cada usuario
        Estatus 03 - Requisicion mayor a $30,000 pesos o $1500 dolares aprobar Thomas
        Estatus 04 - Requisicon revicion equipo finanzas
        Estatus 05 - Requiscion aprobada por Contralo
        Estatus 06 - Requiscion con OC
        Estatus 07 - Almace 
         */
        public Int16 updateState(Int16 _Estatus, String _Folio, String _Editor, String _OC, DateTime _Fecha)
        {
            if (Cnx.getConection().State == ConnectionState.Closed)
            {
                Cnx.getConection().Open();
            }
            //sSQL = "insert into tbTest values ('" + _nombre + "'," + _contador + ")";
            sSQL = "update tbFlujo Set Estatus = " + _Estatus + ", Editor = '" + _Editor + "',OC = '" + _OC + "',FechaModificada ='" + _Fecha.ToString("yyyy/MM/dd HH:mm:ss") + "' where Folio ='" + _Folio + "'";
            try
            {
                oCmd = Cnx.getConection().CreateCommand();
                oCmd.CommandText = sSQL;
                oCmd.CommandTimeout = 0;
                oCmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        
        public Int16 addCenterCost(String _Costo, String _aprobador)
        {
            // Inserta en la tabla tbDescripcionMat el material de la requsicion 
            if (Cnx.getConection().State == ConnectionState.Closed)
            {
                Cnx.getConection().Open();
            }
            sSQL = "insert into tbCentroCosto values ('" + _Costo + "','" + _aprobador + "')";
            try
            {
                oCmd = Cnx.getConection().CreateCommand();
                oCmd.CommandText = sSQL;
                oCmd.CommandTimeout = 0;
                oCmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        
       
        public Int16 deleteTmpTable()
        {

            if (Cnx.getConection().State == ConnectionState.Closed)
            {
                Cnx.getConection().Open();
            }
            sSQL = "DELETE FROM tmpFlujo";
            try
            {
                oCmd = Cnx.getConection().CreateCommand();
                oCmd.CommandText = sSQL;
                oCmd.CommandTimeout = 0;
                oCmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        //Armar la consulta, para mostrar los datos en una tabla
        public DataTable ArmaConsulta(String consulta)
        {
            String Consulta = consulta;
            return obtenerDatos(Consulta);
        }
        //Actualizar
        public Int16 Actualiza(String update)
        {
            if (Cnx.getConection().State == ConnectionState.Closed)
            {
                Cnx.getConection().Open();
            }
            sSQL = update;
            try
            {
                oCmd = Cnx.getConection().CreateCommand();
                oCmd.CommandText = sSQL;
                oCmd.CommandTimeout = 0;
                oCmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

