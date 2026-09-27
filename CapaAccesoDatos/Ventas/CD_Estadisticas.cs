using System.Data;
using CapaAccesoDatos;

namespace CapaAccesoDatos.Ventas
{
    public class CD_Estadisticas
    {
        public DataTable ResumenPedidosPorEstado()
        {
            string sSql = "SELECT Estado, COUNT(*) AS Cantidad, SUM(PrecioTotal) AS Total " +
                "FROM Pedido GROUP BY Estado ORDER BY Estado";
            clsEjecutarComando ejecutar = new clsEjecutarComando();
            return ejecutar.Ejecutar(sSql);
        }

        public DataTable PedidosDelDia()
        {
            string sSql = "SELECT IdPedido, NombreCliente, Cantidad, Estado, PrecioTotal, Fecha " +
                "FROM Pedido WHERE Fecha >= CAST(GETDATE() AS date) AND Fecha < DATEADD(day, 1, CAST(GETDATE() AS date)) ORDER BY IdPedido DESC";
            clsEjecutarComando ejecutar = new clsEjecutarComando();
            return ejecutar.Ejecutar(sSql);
        }
    }
}
