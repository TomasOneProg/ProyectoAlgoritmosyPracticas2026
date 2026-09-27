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
    public class CD_Localidades
    {

        #region ATRIBUTOS
        private int idpartido;
        private int cp;
        private string localidades;
        private int idlocalidad;
        #endregion

        #region PROPERTIES

        public int idLocalidad
        {
            get => idlocalidad;
            set { idlocalidad = value; }
        }

        public string Localidades 
        {
            get => localidades; 
            set {localidades = value;}
        }

        public int CP
        {
            get => cp;
            set {cp = value;}
        }

        public int idPartido
        {
            get => idpartido; 
            set {idpartido = value;}
        }
        #endregion

        #region METODOS

        public DataTable Mostrar()
        {
             string sSql;
             sSql = "Select * from Localidades ";
             clsEjecutarComando Ejecutar = new clsEjecutarComando();
             return Ejecutar.Ejecutar(sSql);  
            
        }

            public void InsertarPersona()
        {
            string sSql = "INSERT INTO Localidades " +
               "(Localidades, CP, idPartido) " +
                "values" +
                " ('" + localidades + "'," + cp + "," + idpartido +
                ")";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void ModificarPersona()
        {
            string sSql = "UPDATE Localidades set " +
                "Localidades='" + localidades + "', CP =" + cp +
                ", idPartido = " + idpartido +
                " WHERE idLocalidad =" + idlocalidad;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarPersona()
        {
            string sSql = "DELETE FROM Localidades WHERE idLocalidad =" + idlocalidad;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }
        
        #endregion
    }
}
