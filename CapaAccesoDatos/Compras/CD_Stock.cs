using System;
using System.Data;
using System.Globalization;
using CapaAccesoDatos;

namespace CapaAccesoDatos.Compras
{
    public class CD_Stock
    {
        #region PROPERTIES
        public int IdStock { get; set; }
        public int IdProducto { get; set; }
        public string NumeroLote { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Precio { get; set; }
        #endregion

        #region METODOS
        public DataTable Mostrar()
        {
            string sSql = "SELECT * FROM Stock ORDER BY IdStock";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            return Ejecutar.Ejecutar(sSql);
        }

        public void InsertarStock()
        {
            string sSql = "INSERT INTO Stock " +
                "(IdProducto, NumeroLote, Cantidad, FechaVencimiento, Precio) VALUES (" +
                IdProducto + ",'" + T(NumeroLote) + "'," + Cantidad + ",#" + Fecha() + "#," + Num(Precio) + ")";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void ModificarStock()
        {
            string sSql = "UPDATE Stock SET " +
                "IdProducto=" + IdProducto + ", NumeroLote='" + T(NumeroLote) + "', Cantidad=" + Cantidad +
                ", FechaVencimiento=#" + Fecha() + "#, Precio=" + Num(Precio) + " " +
                "WHERE IdStock=" + IdStock;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarStock()
        {
            string sSql = "DELETE FROM Stock WHERE IdStock=" + IdStock;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        private string T(string valor)
        {
            return (valor ?? "").Replace("'", "''");
        }

        // Formato de fecha ISO para SQL Server (yyyy-MM-dd)
        private string Fecha()
        {
            return FechaVencimiento.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        // Numero con punto decimal, independiente de la cultura/idioma activo
        private string Num(decimal valor)
        {
            return valor.ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
