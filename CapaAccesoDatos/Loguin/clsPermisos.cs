using System;
using System.Data;
using CapaComun;
using CapaAccesoDatos;

namespace CapaDatos
{
    public class clsPermisos : clsConexion
    {
        private const string FiltroUsuarioActivo =
            " AND PermisosUsuarios.FechaBaja IS NULL " +
            " AND (PermisosUsuarios.AltaProvisoria IS NULL OR PermisosUsuarios.AltaProvisoria >= CAST(GETDATE() AS date)) ";

        public bool Permisos(int idUser)
        {
            UserCache.PermisosUsuario.Clear();

            bool tieneAlguno = false;
            tieneAlguno |= CargarPermisosDirectos(idUser);
            tieneAlguno |= CargarPermisosPorGrupo(idUser);
            CargarGrupoUsuario(idUser);

            return tieneAlguno;
        }

        private void CargarGrupoUsuario(int idUser)
        {
            UserCache.IdGrupo = 0;
            UserCache.NombreGrupo = null;

            string sSql = "SELECT UsuariosGrupos.IdGrupo, Grupos.Grupo " +
                "FROM UsuariosGrupos INNER JOIN Grupos ON UsuariosGrupos.IdGrupo = Grupos.IdGrupo " +
                "WHERE UsuariosGrupos.IdUsuario = " + idUser;

            clsEjecutarComando ejecutar = new clsEjecutarComando();
            DataTable dt = ejecutar.Ejecutar(sSql);
            if (dt.Rows.Count > 0)
            {
                UserCache.IdGrupo = Convert.ToInt32(dt.Rows[0][0]);
                UserCache.NombreGrupo = dt.Rows[0][1].ToString();
            }
        }

        private bool CargarPermisosDirectos(int idUser)
        {
            string sSql = "SELECT PermisosUsuarios.IdPermiso, Permisos.Funcionalidad " +
                "FROM Permisos INNER JOIN PermisosUsuarios ON Permisos.IdPermiso = PermisosUsuarios.IdPermiso " +
                "WHERE PermisosUsuarios.IdUsuario = " + idUser + FiltroUsuarioActivo;

            clsEjecutarComando ejecutar = new clsEjecutarComando();
            return AgregarPermisos(ejecutar.Ejecutar(sSql));
        }

        private bool CargarPermisosPorGrupo(int idUser)
        {
            string sSql = "SELECT Permisos.IdPermiso, Permisos.Funcionalidad " +
                "FROM UsuariosGrupos " +
                "INNER JOIN PermisosGrupos ON UsuariosGrupos.IdGrupo = PermisosGrupos.IdGrupo " +
                "INNER JOIN Permisos ON PermisosGrupos.IdPermiso = Permisos.IdPermiso " +
                "WHERE UsuariosGrupos.IdUsuario = " + idUser;

            clsEjecutarComando ejecutar = new clsEjecutarComando();
            return AgregarPermisos(ejecutar.Ejecutar(sSql));
        }

        private bool AgregarPermisos(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            foreach (DataRow row in dt.Rows)
            {
                int idPermiso = Convert.ToInt32(row[0]);
                string funcionalidad = row[1].ToString();
                if (!UserCache.PermisosUsuario.ContainsKey(idPermiso))
                {
                    UserCache.PermisosUsuario.Add(idPermiso, funcionalidad);
                }
            }
            return true;
        }
    }
}
