using System;
using System.Data;
using System.Globalization;
using CapaAccesoDatos;

namespace CapaAccesoDatos.Ventas
{
    public class CD_Menu
    {
        #region PROPERTIES
        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string Ingredientes { get; set; }
        public decimal Precio { get; set; }
        public string Region { get; set; }
        public string Temporada { get; set; }
        public string Popularidad { get; set; }
        public int TiempoPreparacion { get; set; }
        public string TipoEvento { get; set; }
        public int IdStock { get; set; }
        #endregion

        #region METODOS
        public DataTable Mostrar()
        {
            string sSql = "SELECT * FROM Menu ORDER BY Nombre";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            return Ejecutar.Ejecutar(sSql);
        }

        public int InsertarMenu()
        {
            string sSql = "INSERT INTO Menu " +
                "(Nombre, Descripcion, Categoria, Ingredientes, Precio, Region, Temporada, Popularidad, TiempoPreparacion, TipoEvento, IdStock) VALUES (" +
                "'" + T(Nombre) + "','" + T(Descripcion) + "','" + T(Categoria) + "','" + T(Ingredientes) + "'," +
                Num(Precio) + ",'" + T(Region) + "','" + T(Temporada) + "','" + T(Popularidad) + "'," +
                TiempoPreparacion + ",'" + T(TipoEvento) + "'," + IdStock + ")";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
            DataTable dt = Ejecutar.Ejecutar("SELECT @@IDENTITY AS IdMenu");
            return Convert.ToInt32(dt.Rows[0]["IdMenu"]);
        }

        public void ModificarMenu()
        {
            string sSql = "UPDATE Menu SET " +
                "Nombre='" + T(Nombre) + "', Descripcion='" + T(Descripcion) + "', Categoria='" + T(Categoria) +
                "', Ingredientes='" + T(Ingredientes) + "', Precio=" + Num(Precio) + ", Region='" + T(Region) +
                "', Temporada='" + T(Temporada) + "', Popularidad='" + T(Popularidad) + "', TiempoPreparacion=" + TiempoPreparacion +
                ", TipoEvento='" + T(TipoEvento) + "', IdStock=" + IdStock + " " +
                "WHERE IdMenu=" + IdMenu;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarMenu()
        {
            AsegurarTablaReceta();
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.EjecucionDirecta("DELETE FROM RecetaMenu WHERE IdMenu=" + IdMenu);
            Ejecutar.EjecucionDirecta("DELETE FROM Menu WHERE IdMenu=" + IdMenu);
        }

        public void AsegurarTablaReceta()
        {
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            try
            {
                Ejecutar.Ejecutar("SELECT TOP 1 * FROM RecetaMenu");
            }
            catch
            {
                Ejecutar.EjecucionDirecta(
                    "CREATE TABLE RecetaMenu (IdReceta AUTOINCREMENT PRIMARY KEY, IdMenu INTEGER NOT NULL, IdStock INTEGER NOT NULL, CantidadConsumir DOUBLE NOT NULL)");
            }
        }

        public DataTable ObtenerStockParaReceta()
        {
            string sSql = "SELECT S.IdStock, S.NumeroLote, S.Cantidad AS CantidadDisponible, " +
                          "P.Nombre AS Producto, P.Medida AS Unidad " +
                          "FROM Stock AS S INNER JOIN Producto AS P ON S.IdProducto=P.IdProducto " +
                          "ORDER BY P.Nombre, S.NumeroLote";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            return Ejecutar.Ejecutar(sSql);
        }

        public DataTable ObtenerReceta(int idMenu)
        {
            AsegurarTablaReceta();
            string sSql = "SELECT R.IdStock, S.NumeroLote, P.Nombre AS Producto, P.Medida AS Unidad, " +
                          "S.Cantidad AS CantidadDisponible, R.CantidadConsumir " +
                          "FROM (RecetaMenu AS R INNER JOIN Stock AS S ON R.IdStock=S.IdStock) " +
                          "INNER JOIN Producto AS P ON S.IdProducto=P.IdProducto " +
                          "WHERE R.IdMenu=" + idMenu + " ORDER BY R.IdReceta";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            return Ejecutar.Ejecutar(sSql);
        }

        public void GuardarReceta(int idMenu, DataTable receta)
        {
            AsegurarTablaReceta();
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.EjecucionDirecta("DELETE FROM RecetaMenu WHERE IdMenu=" + idMenu);

            foreach (DataRow row in receta.Rows)
            {
                int idStock = Convert.ToInt32(row["IdStock"]);
                decimal cantidad = Convert.ToDecimal(row["CantidadConsumir"], CultureInfo.InvariantCulture);
                if (cantidad <= 0) continue;

                string sql = "INSERT INTO RecetaMenu (IdMenu, IdStock, CantidadConsumir) VALUES (" +
                             idMenu + "," + idStock + "," + Num(cantidad) + ")";
                Ejecutar.EjecucionDirecta(sql);
            }
        }

        private string T(string valor)
        {
            return (valor ?? "").Replace("'", "''");
        }

        private string Num(decimal valor)
        {
            return valor.ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
