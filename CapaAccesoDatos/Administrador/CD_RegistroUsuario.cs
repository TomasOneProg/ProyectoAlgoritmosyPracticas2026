using System;
using System.Data;
using CapaAccesoDatos;

namespace CapaAccesoDatos.Administrador
{
    public class CD_RegistroUsuario
    {
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int IdTipoDoc { get; set; }
        public int NroDoc { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int IdLocalidad { get; set; }
        public int IdCargo { get; set; }
        public string Usuario { get; set; }
        public string PasswordHash { get; set; }
        public int IdGrupo { get; set; }

        private static string SqlTxt(string valor)
        {
            return (valor ?? "").Replace("'", "''");
        }

        public bool ExisteUsuario(string nombreUsuario)
        {
            string sSql = "SELECT IdUsuario FROM Usuarios WHERE [Usuario] = '" + SqlTxt(nombreUsuario.Trim()) + "'";
            clsEjecutarComando ejecutar = new clsEjecutarComando();
            DataTable dt = ejecutar.Ejecutar(sSql);
            return dt.Rows.Count > 0;
        }

        public bool ExisteDocumento(int idTipoDoc, int nroDoc)
        {
            return ObtenerIdPersonaPorDocumento(idTipoDoc, nroDoc) > 0;
        }

        public int ObtenerIdPersonaPorDocumento(int idTipoDoc, int nroDoc)
        {
            string sSql = "SELECT IdPersona FROM Personal WHERE IdTDoc = " + idTipoDoc +
                " AND NroDoc = " + nroDoc;
            clsEjecutarComando ejecutar = new clsEjecutarComando();
            DataTable dt = ejecutar.Ejecutar(sSql);
            if (dt.Rows.Count == 0 || dt.Rows[0][0] == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public bool ExisteUsuarioPorPersona(int idPersona)
        {
            string sSql = "SELECT IdUsuario FROM Usuarios WHERE IdPersona = " + idPersona;
            clsEjecutarComando ejecutar = new clsEjecutarComando();
            DataTable dt = ejecutar.Ejecutar(sSql);
            return dt.Rows.Count > 0;
        }

        /// <param name="idPersonaExistente">Si es mayor a 0, no inserta en Personal y usa ese IdPersona.</param>
        public int Registrar(int idPersonaExistente = 0)
        {
            clsEjecutarComando ejecutar = new clsEjecutarComando();
            int idPersona = idPersonaExistente;
            bool personalNuevo = idPersona <= 0;

            try
            {
                if (personalNuevo)
                {
                    int idCargo = IdCargo > 0 ? IdCargo : 6;
                    int idLocalidad = IdLocalidad > 0 ? IdLocalidad : 1;

                    string sqlPersonal = "INSERT INTO Personal " +
                        "(Apellido, Nombres, IdTDoc, NroDoc, Telefono, Correo, Calle, Nro, Piso, Dto, IdLocalidad, IdCargo) " +
                        "VALUES ('" + SqlTxt(Apellido) + "','" + SqlTxt(Nombres) + "'," + IdTipoDoc + "," + NroDoc +
                        ",'" + SqlTxt(Telefono) + "','" + SqlTxt(Correo) + "','','0','',''," +
                        idLocalidad + "," + idCargo + ")";
                    ejecutar.EjecucionDirecta(sqlPersonal);

                    idPersona = ObtenerMaxId(ejecutar, "Personal", "IdPersona");
                    if (idPersona <= 0)
                    {
                        throw new InvalidOperationException("No se pudo obtener el IdPersona generado.");
                    }
                }

                if (ExisteUsuarioPorPersona(idPersona))
                {
                    throw new InvalidOperationException("Esta persona ya tiene un usuario de acceso en el sistema.");
                }

                string fechaAlta = DateTime.Today.ToString("yyyy-MM-dd");
                string sqlUsuario = "INSERT INTO Usuarios " +
                    "([Usuario], [Password], [IdPersona], [FechaAlta], [CambiaCada], [UsuarioDesactivado]) " +
                    "VALUES ('" + SqlTxt(Usuario.Trim()) + "','" + PasswordHash + "'," + idPersona +
                    ",'" + fechaAlta + "',90,0)";
                ejecutar.EjecucionDirecta(sqlUsuario);

                int idUsuario = ObtenerMaxId(ejecutar, "Usuarios", "IdUsuario");
                if (idUsuario <= 0)
                {
                    throw new InvalidOperationException("No se pudo obtener el IdUsuario generado.");
                }

                if (IdGrupo > 0)
                {
                    ejecutar.EjecucionDirecta(
                        "INSERT INTO UsuariosGrupos (IdUsuario, IdGrupo) VALUES (" + idUsuario + ", " + IdGrupo + ")");
                }

                return idUsuario;
            }
            catch
            {
                if (personalNuevo && idPersona > 0)
                {
                    try
                    {
                        ejecutar.EjecucionDirecta("DELETE FROM Personal WHERE IdPersona = " + idPersona);
                    }
                    catch
                    {
                    }
                }
                throw;
            }
        }

        private static int ObtenerMaxId(clsEjecutarComando ejecutar, string tabla, string campo)
        {
            DataTable dt = ejecutar.Ejecutar("SELECT MAX(" + campo + ") AS Ultimo FROM " + tabla);
            if (dt.Rows.Count == 0 || dt.Rows[0]["Ultimo"] == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt32(dt.Rows[0]["Ultimo"]);
        }
    }
}
