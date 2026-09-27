using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoDatos.Loguin
{
    public class CD_BloqueoUser
    {

        clsEjecutarComando Ejecutar = new clsEjecutarComando();
        public void BloquearUser(string User)
        {
            string SSql = "UPDATE Usuarios SET Usuarios.UsuarioDesactivado = 1 WHERE Usuarios.Usuario ='" + User +"'";
            Ejecutar.EjecucionDirecta(SSql);
        }
    }
}
