using System;
using System.Data;
using CapaAccesoDatos;

namespace CapaAccesoDatos.Administrador
{
    public class CD_Bitacora
    {
        private static string SqlTxt(string valor)
        {
            return (valor ?? "").Replace("'", "''");
        }

        public DataTable Mostrar()
        {
            return MostrarFiltrada(null, null, null, null);
        }

        public DataTable MostrarFiltrada(string evento, string usuario, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            string sSql = "SELECT * FROM Bitacora WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(evento))
            {
                sSql += " AND Evento LIKE '%" + SqlTxt(evento.Trim()) + "%'";
            }

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                sSql += " AND Usuario LIKE '%" + SqlTxt(usuario.Trim()) + "%'";
            }

            if (fechaDesde.HasValue)
            {
                sSql += " AND Fecha >= '" + fechaDesde.Value.ToString("yyyy-MM-dd") + "'";
            }

            if (fechaHasta.HasValue)
            {
                sSql += " AND Fecha <= '" + fechaHasta.Value.ToString("yyyy-MM-dd") + "'";
            }

            sSql += " ORDER BY Fecha DESC, Hora DESC";
            clsEjecutarComando ejecutar = new clsEjecutarComando();
            return ejecutar.Ejecutar(sSql);
        }
    }
}
