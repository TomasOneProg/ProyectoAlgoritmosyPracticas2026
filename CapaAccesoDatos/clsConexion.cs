using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CapaAccesoDatos
{
    public abstract class clsConexion
    {
        private readonly string cadena;

        public clsConexion()
        {
            var configuracion = ConfigurationManager.ConnectionStrings["SistemaLoguin"];
            if (configuracion == null || string.IsNullOrWhiteSpace(configuracion.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "No se encontró la cadena de conexión 'SistemaLoguin' en el archivo de configuración.");
            }

            cadena = configuracion.ConnectionString;
        }

        protected SqlConnection GetConexion()
        {
            return new SqlConnection(cadena);
        }
    }
}
