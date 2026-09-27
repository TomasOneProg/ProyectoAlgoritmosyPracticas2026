using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaComun;

namespace CapaAccesoDatos
{
    public class CD_clsBitacora
    {
        public CD_clsBitacora(string evento, string detalle, string origen)
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string hora = DateTime.Now.ToString("HH:mm");
            int IdUsuario; 
            string usuario;

            if (UserCache.IdUsuario == 0)
            {  IdUsuario = VerificCache.IdUsuario; }
            else
            {  IdUsuario = UserCache.IdUsuario; }

            if (UserCache.Apellido == null)
            { usuario = VerificCache.Apellido + " " + VerificCache.Nombres; }
            else
            { usuario = UserCache.Apellido + " " + UserCache.Nombres; }

            string sSQL = "INSERT INTO Bitacora (Fecha, Hora, IdUsuario, Usuario, Evento, Detalle, Origen) " +
                "VALUES ('" + fecha + "', '" + hora + "', " + IdUsuario + ", '" + SqlTxt(usuario) +
                "', '" + SqlTxt(evento) + "', '" + SqlTxt(detalle) + "', '" + SqlTxt(origen) + "')";

            clsEjecutarComando Ejecutar = new clsEjecutarComando();
            Ejecutar.EjecucionDirecta(sSQL);
        }

        private static string SqlTxt(string valor)
        {
            return (valor ?? "").Replace("'", "''");
        }
    }
}
