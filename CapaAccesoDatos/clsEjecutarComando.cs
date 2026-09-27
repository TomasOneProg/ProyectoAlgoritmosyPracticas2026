using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaAccesoDatos
{
    class clsEjecutarComando : clsConexion
    {
        [ThreadStatic]
        private static bool registrandoEnBitacora;

        private static void RegistrarErrorSql(Exception ex, string sSql)
        {
            if (registrandoEnBitacora)
            {
                return;
            }

            try
            {
                registrandoEnBitacora = true;
                string detalle = ex.Message;
                if (detalle.Length > 400)
                {
                    detalle = detalle.Substring(0, 400) + "...";
                }

                string origen = "clsEjecutarComando";
                if (!string.IsNullOrEmpty(sSql) && sSql.Length > 80)
                {
                    detalle += " | SQL: " + sSql.Substring(0, 80) + "...";
                }
                else if (!string.IsNullOrEmpty(sSql))
                {
                    detalle += " | SQL: " + sSql;
                }

                new CD_clsBitacora("Error SQL", detalle, origen);
            }
            catch
            {
            }
            finally
            {
                registrandoEnBitacora = false;
            }
        }

        public DataTable Ejecutar(string sSql)
        {
            try
            {
                using (SqlConnection CNN = GetConexion())
                {
                    CNN.Open();

                    using (SqlCommand comando = new SqlCommand(sSql, CNN))
                    using (SqlDataReader DR = comando.ExecuteReader())
                    {
                        DataTable DT = new DataTable();
                        DT.Load(DR);
                        return DT;
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErrorSql(ex, sSql);
                throw;
            }
        }

        public void EjecucionDirecta(string sSql)
        {
            try
            {
                using (SqlConnection CNN = GetConexion())
                {
                    CNN.Open();

                    using (SqlCommand comando = new SqlCommand(sSql, CNN))
                    {
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                RegistrarErrorSql(ex, sSql);
                throw;
            }
        }
    }
}
