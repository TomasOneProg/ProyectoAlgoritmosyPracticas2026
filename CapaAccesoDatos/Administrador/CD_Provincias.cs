using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaComun;
using CapaAccesoDatos;
using CapaAccesoDatos.Administrador;

namespace CapaAccesoDatos.Administrador
{
    public class CD_Provincias
    {


        #region ATRIBUTOS
        private string provincia;
        private int idprovincia;
        #endregion

        #region PROPERTIES

        public string Provincia
        {
            get => provincia; 
            set {provincia = value;}
        }

        public int IdProvincia
        {
            get => idprovincia;
            set {idprovincia = value;}
        }
        #endregion

        #region METODOS

        public DataTable Mostrar()
        {
            
            string sSql;
             sSql = "Select * from Provincias ";
             clsEjecutarComando Ejecutar = new clsEjecutarComando();
             return Ejecutar.Ejecutar(sSql);

        }
        
            public void InsertarProvincia()
        {
            string sSql = "INSERT INTO Provincias " +
                "(Provincia)" +
                "values" +
                " ('" +provincia + "')";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void ModificarProvincia()
        {
            string sSql = "UPDATE Provincias set " +
                " Provincia = '" + provincia + "'" +
                " WHERE IdProvincia =" + idprovincia;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarProvincia()
        {
            string sSql = "DELETE FROM Provincias WHERE IdProvincia =" + idprovincia;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        #endregion
    }
}
