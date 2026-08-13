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
            string sSql = "SELECT S.*, P.Nombre AS NombreProducto, P.Medida AS UnidadMedida " +
                          "FROM Stock AS S LEFT JOIN Producto AS P ON S.IdProducto=P.IdProducto ORDER BY S.IdStock";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            return Ejecutar.Ejecutar(sSql);
        }

        public void InsertarStock()
        {
            string sSql = "INSERT INTO Stock (IdProducto, NumeroLote, Cantidad, FechaVencimiento, Precio) VALUES (" +
                IdProducto + ",'" + T(NumeroLote) + "'," + Cantidad + ",#" + Fecha() + "#," + Num(Precio) + ")";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void ModificarStock()
        {
            string sSql = "UPDATE Stock SET IdProducto=" + IdProducto + ", NumeroLote='" + T(NumeroLote) + "', Cantidad=" + Cantidad +
                ", FechaVencimiento=#" + Fecha() + "#, Precio=" + Num(Precio) + " WHERE IdStock=" + IdStock;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarStock()
        {
            string sSql = "DELETE FROM Stock WHERE IdStock=" + IdStock;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public DataTable ObtenerStockParaConsumo(int idMenu)
        {
            string sSql = "SELECT R.IdStock, S.NumeroLote, P.Nombre AS Producto, P.Medida AS Unidad, " +
                          "S.Cantidad AS CantidadDisponible, R.CantidadConsumir " +
                          "FROM (RecetaMenu AS R INNER JOIN Stock AS S ON R.IdStock=S.IdStock) " +
                          "INNER JOIN Producto AS P ON S.IdProducto=P.IdProducto WHERE R.IdMenu=" + idMenu;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            return Ejecutar.Ejecutar(sSql);
        }

        public void ConsumirReceta(int idMenu, int cantidadPlatos)
        {
            DataTable receta = ObtenerStockParaConsumo(idMenu);
            if (receta.Rows.Count == 0) return;

            foreach (DataRow row in receta.Rows)
            {
                decimal porPlato = Convert.ToDecimal(row["CantidadConsumir"]);
                decimal total = porPlato * cantidadPlatos;
                decimal disponible = Convert.ToDecimal(row["CantidadDisponible"]);
                if (total > disponible)
                    throw new InvalidOperationException("Stock insuficiente para el lote " + row["NumeroLote"] +
                        ". Disponible: " + disponible + " " + row["Unidad"] + ". Requerido: " + total + " " + row["Unidad"] + ".");
            }

            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            foreach (DataRow row in receta.Rows)
            {
                decimal porPlato = Convert.ToDecimal(row["CantidadConsumir"]);
                decimal total = porPlato * cantidadPlatos;
                int idStock = Convert.ToInt32(row["IdStock"]);
                Ejecutar.EjecucionDirecta("UPDATE Stock SET Cantidad=Cantidad-" + Num(total) +
                    " WHERE IdStock=" + idStock + " AND Cantidad>=" + Num(total));
            }
        }

        private string T(string valor) { return (valor ?? "").Replace("'", "''"); }
        private string Fecha() { return FechaVencimiento.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); }
        private string Num(decimal valor) { return valor.ToString(CultureInfo.InvariantCulture); }
        #endregion
    }
}
