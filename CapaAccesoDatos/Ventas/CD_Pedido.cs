using System;
using System.Data;
using System.Globalization;
using CapaAccesoDatos;

namespace CapaAccesoDatos.Ventas
{
    public class CD_Pedido
    {
        #region PROPERTIES
        public int IdPedido { get; set; }
        public string NombreCliente { get; set; }
        public int IdMenu { get; set; }
        public int Cantidad { get; set; }
        public string FormaPago { get; set; }
        public string InstruccionesEspeciales { get; set; }
        public string Estado { get; set; }
        public decimal PrecioTotal { get; set; }
        public DateTime Fecha { get; set; }
        #endregion

        #region METODOS
        public DataTable Mostrar()
        {
            string sSql = "SELECT * FROM Pedido ORDER BY IdPedido DESC";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            return Ejecutar.Ejecutar(sSql);
        }

        public void InsertarPedido()
        {
            string sSql = "INSERT INTO Pedido " +
                "(NombreCliente, IdMenu, Cantidad, FormaPago, InstruccionesEspeciales, Estado, PrecioTotal, Fecha) VALUES (" +
                "'" + T(NombreCliente) + "'," + IdMenu + "," + Cantidad + ",'" + T(FormaPago) + "','" +
                T(InstruccionesEspeciales) + "','" + T(Estado) + "'," + Num(PrecioTotal) + ",'" + FechaTxt() + "')";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void ModificarPedido()
        {
            string sSql = "UPDATE Pedido SET " +
                "NombreCliente='" + T(NombreCliente) + "', IdMenu=" + IdMenu + ", Cantidad=" + Cantidad +
                ", FormaPago='" + T(FormaPago) + "', InstruccionesEspeciales='" + T(InstruccionesEspeciales) +
                "', Estado='" + T(Estado) + "', PrecioTotal=" + Num(PrecioTotal) + ", Fecha='" + FechaTxt() + "' " +
                "WHERE IdPedido=" + IdPedido;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarPedido()
        {
            string sSql = "DELETE FROM Pedido WHERE IdPedido=" + IdPedido;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        // Cobrar pedido: marca el pedido como Cobrado
        public void CobrarPedido()
        {
            string sSql = "UPDATE Pedido SET Estado='Cobrado' WHERE IdPedido=" + IdPedido;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        private string T(string valor)
        {
            return (valor ?? "").Replace("'", "''");
        }

        private string FechaTxt()
        {
            return Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private string Num(decimal valor)
        {
            return valor.ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
