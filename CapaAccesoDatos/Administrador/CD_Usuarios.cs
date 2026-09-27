using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaComun;
using CapaAccesoDatos;

namespace CapaAccesoDatos.Administrador
{
    public class CD_Usuarios
    {


        #region ATRIBUTOS
        private int idUsuario;
        private string usuario;
        private string password;
        private int idPersona;
        private DateTime fechaAlta;// modificar tipo
        private DateTime fechaBaja;// modificar tipo
        private int cambiaCada;
        private DateTime fechaUltimoCambio;// modificar tipo
        private bool usuarioDesactivado;
        private DateTime fechaDesactivacion;// modificar tipo
        #endregion

        #region PROPERTIES

        public int IdUsuario
        { get; set; }

        public string Usuario
        { get; set; }

        public string Password
        { get; set; }

        public int IdPersona
        { get; set; }

        public DateTime FechaAlta
        { get; set; }

        public DateTime FechaBaja
        { get; set; }

        public int CambiaCada
        { get; set; }

        public DateTime UltimoCambio
        { get; set; }

        public bool UsuarioDesactivado
        { get; set; }

        public DateTime FechaDesativacion
        { get; set; }

        #endregion

        #region METODOS

        public DataTable Mostrar()
        {
             string sSql;
             sSql = "SELECT * FROM Usuarios";
             clsEjecutarComando Ejecutar = new clsEjecutarComando();
             return Ejecutar.Ejecutar(sSql);  
            
        }

          
        public void InsertarUsuario()
        {
            string sSql = "INSERT INTO Usuarios " +
               "(Usuario, Password, IdPersona, FechaAlta, FechaBaja, CambiaCada, FechaUltimoCambio, UsuarioDesactivado, FechaDesactivacion) " +
                "VALUES ('" + usuario + "','" + password + "'," + idPersona + ",'" +
                fechaAlta.ToString("yyyy-MM-dd") + "','" + fechaBaja.ToString("yyyy-MM-dd") + "'," + cambiaCada +
                ",'" + fechaUltimoCambio.ToString("yyyy-MM-dd") + "'," + (usuarioDesactivado ? "1" : "0") +
                ",'" + fechaDesactivacion.ToString("yyyy-MM-dd") + "')";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        
        public void ModificarUsuario()
        {
            string sSql = "UPDATE Usuarios set " +
                "Usuario='" + usuario + "', Password='" + password  + "', IdPersona =" + idPersona  +
                ", FechaAlta = '" + fechaAlta.ToString("yyyy-MM-dd") + "', FechaBaja = '" + fechaBaja.ToString("yyyy-MM-dd") +
                "', CambiaCada = " + cambiaCada + ", FechaUltimoCambio = '" + fechaUltimoCambio.ToString("yyyy-MM-dd") +
                "', UsuarioDesactivado = " + (usuarioDesactivado ? "1" : "0") +
                ", FechaDesactivacion = '" + FechaDesativacion.ToString("yyyy-MM-dd") +
                "' WHERE IdUsuario =" + idUsuario;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarUsuario()
        {
            string sSql = "DELETE FROM Usuarios WHERE IdUsuario =" + idUsuario;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }
        
        #endregion
    }
}
