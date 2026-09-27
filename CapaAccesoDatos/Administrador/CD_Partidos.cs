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
    public class CD_Partidos
    {

        #region ATRIBUTOS
        private int idpartido;
        private string partido;
        private int idprovincia;
        #endregion

        #region PROPERTIES

        public int idPartido
        {
            get => idpartido;
            set { idpartido = value; }
        }

        public string Partido
        {
            get => partido; 
            set {partido = value;}
        }

        public int idProvincia
        {
            get => idprovincia;
            set {idprovincia = value;}
        }
        #endregion

        #region METODOS

        public DataTable Mostrar()
        {
             string sSql;
             sSql = "Select * from Partidos ";
             clsEjecutarComando Ejecutar = new clsEjecutarComando();
             return Ejecutar.Ejecutar(sSql);  
            
        }

            public void InsertarPersona()
        {
            string sSql = "INSERT INTO Partidos " +
               "(Partido, IdProvincia ) " +
                "values" +
                " ('" + partido + "'," + idprovincia + ")";
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void ModificarPersona()
        {
            string sSql = "UPDATE Partidos set " +
                "IdPartido='" + idpartido + "', Partido='" + partido + "', IdProvincia = " + idprovincia +
                " WHERE IdPartido =" + idpartido;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }

        public void EliminarPersona()
        {
            string sSql = "DELETE FROM Partidos WHERE IdPartido =" + idpartido;
            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.Ejecutar(sSql);
        }
        
        #endregion
    }
}
