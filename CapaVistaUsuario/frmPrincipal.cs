using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaComun;


namespace CapaVistaUsuario
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            AplicarIdioma();
            AplicarPermisos();
        }

        // Muestra u oculta menús según permisos cargados al loguearse
        private void AplicarPermisos()
        {
            bool admin = PermisosSistema.EsAdministrador();

            comprasToolStripMenuItem.Visible = admin || PermisosSistema.TieneAlguno(
                PermisosSistema.Proveedores, PermisosSistema.Productos, PermisosSistema.Stock);
            proveedoresToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Proveedores);
            productosToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Productos);
            stockComprasToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Stock);

            facturacionToolStripMenuItem.Visible = admin || PermisosSistema.TieneAlguno(
                PermisosSistema.Menu, PermisosSistema.Pedidos);
            menuToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Menu);
            pedidosToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Pedidos);

            administracionToolStripMenuItem.Visible = admin || PermisosSistema.TieneAlguno(
                PermisosSistema.AltaUsuarios, PermisosSistema.BajaUsuarios,
                PermisosSistema.ModificacionUsuarios, PermisosSistema.Personal,
                PermisosSistema.Bitacora, PermisosSistema.Ubicaciones);
            gestiónDePersonalToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Personal);
            registroToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Bitacora);
            bitacoraToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Bitacora);

            mantenimientoToolStripMenuItem.Visible = admin || PermisosSistema.Tiene(PermisosSistema.Ubicaciones);

            usuariosToolStripMenuItem.Visible = true;
            cambioDePasswordToolStripMenuItem.Visible = true;

            gestionDeUsuariosToolStripMenuItem.Visible = PermisosSistema.PuedeGestionarUsuarios();
            asignarRolToolStripMenuItem.Visible = PermisosSistema.PuedeAsignarRoles();

            estadisticasToolStripMenuItem.Visible = PermisosSistema.PuedeVerEstadisticas();

            if (!string.IsNullOrEmpty(UserCache.NombreGrupo))
            {
                lblSesion.Text += " | " + Idioma.Texto("Rol") + ": " + UserCache.NombreGrupo;
            }
        }

        private bool VerificarPermiso(string permiso)
        {
            if (PermisosSistema.Tiene(permiso) || PermisosSistema.EsAdministrador())
            {
                return true;
            }
            MessageBox.Show(Idioma.Texto("Msg_SinPermiso"));
            return false;
        }

        // Aplica el idioma activo a los textos del formulario principal
        private void AplicarIdioma()
        {
            lblSesion.Text = Idioma.Texto("Sesion") + " " + UserCache.Apellido + " " + UserCache.Nombres;

            mantenimientoToolStripMenuItem.Text = Idioma.Texto("Mantenimiento");
            usuariosToolStripMenuItem.Text = Idioma.Texto("Usuarios");
            cambioDePasswordToolStripMenuItem.Text = Idioma.Texto("CambioPassword");
            mantenimientoDeUsuariosToolStripMenuItem.Text = Idioma.Texto("MantenimientoUsuarios");
            salirToolStripMenuItem.Text = Idioma.Texto("Salir");
            facturacionToolStripMenuItem.Text = Idioma.Texto("Facturacion");
            ventasToolStripMenuItem.Text = Idioma.Texto("Ventas");
            stockToolStripMenuItem.Text = Idioma.Texto("Stock");
            administracionToolStripMenuItem.Text = Idioma.Texto("Administracion");
            registroToolStripMenuItem.Text = Idioma.Texto("Registros");
            bitacoraToolStripMenuItem.Text = Idioma.Texto("Bitacora");
            ventasToolStripMenuItem1.Text = Idioma.Texto("Ventas");
            gestionDeUsuariosToolStripMenuItem.Text = Idioma.Texto("RegistrarUsuario");
            gestiónDePersonalToolStripMenuItem.Text = Idioma.Texto("GestionPersonal");
            idiomaToolStripMenuItem.Text = Idioma.Texto("Idioma");
            comprasToolStripMenuItem.Text = Idioma.Texto("Compras");
            proveedoresToolStripMenuItem.Text = Idioma.Texto("Proveedores");
            productosToolStripMenuItem.Text = Idioma.Texto("Productos");
            stockComprasToolStripMenuItem.Text = Idioma.Texto("Stock");
            facturacionToolStripMenuItem.Text = Idioma.Texto("Ventas");
            menuToolStripMenuItem.Text = Idioma.Texto("Menu");
            pedidosToolStripMenuItem.Text = Idioma.Texto("Pedidos");
            estadisticasToolStripMenuItem.Text = Idioma.Texto("Estadisticas");
            asignarRolToolStripMenuItem.Text = Idioma.Texto("AsignarRol");
        }

        private void estadisticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PermisosSistema.PuedeVerEstadisticas())
            {
                MessageBox.Show(Idioma.Texto("Msg_SinPermiso"));
                return;
            }
            Ventas.frmEstadisticas fAux = new Ventas.frmEstadisticas();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void asignarRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PermisosSistema.PuedeAsignarRoles())
            {
                MessageBox.Show(Idioma.Texto("Msg_SinPermiso"));
                return;
            }
            Administrador.frmAsignarRol fAux = new Administrador.frmAsignarRol();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PermisosSistema.Tiene(PermisosSistema.Bitacora) && !PermisosSistema.EsAdministrador())
            {
                MessageBox.Show(Idioma.Texto("Msg_SinPermiso"));
                return;
            }
            Administrador.frmBitacora fAux = new Administrador.frmBitacora();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!VerificarPermiso(PermisosSistema.Proveedores)) return;
            Compras.frmProveedores fAux = new Compras.frmProveedores();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!VerificarPermiso(PermisosSistema.Productos)) return;
            Compras.frmProducto fAux = new Compras.frmProducto();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void stockComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!VerificarPermiso(PermisosSistema.Stock)) return;
            Compras.frmStock fAux = new Compras.frmStock();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!VerificarPermiso(PermisosSistema.Menu)) return;
            Ventas.frmMenu fAux = new Ventas.frmMenu();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!VerificarPermiso(PermisosSistema.Pedidos)) return;
            Ventas.frmFacturacion fAux = new Ventas.frmFacturacion();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void espanolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Idioma.Establecer(Idioma.Espanol);
            AplicarIdioma();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Idioma.Establecer(Idioma.Ingles);
            AplicarIdioma();
        }

        private void portuguesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Idioma.Establecer(Idioma.Portugues);
            AplicarIdioma();
        }

        private void cambioDePasswordToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Loguin.frmCambioPassword fAux = new Loguin.frmCambioPassword();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        /*
         * El evento FormClosing se produce al momento de cerrar el formulario, en este evento
         * que se dispara a través de una instruccion o bien al clickear sobre la X de los ControlBox
         * del formulario, es donde colocamos lo que queremos que suceda al cerrar
         * */
        {
            //pregunto si desea cerrar, dando la opción de cancelar la operación
            DialogResult resultado = MessageBox.Show("Esta seguro de CERRAR el sistema?",
                                "FINALIZAR SISTEMA",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Asterisk,
                                MessageBoxDefaultButton.Button2);

            if (resultado == DialogResult.Cancel)
            {
                e.Cancel = true; //Cancela el cierre del formulario
            }
        }

        private void gestiónDePersonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!VerificarPermiso(PermisosSistema.Personal)) return;
            frmPersonas fAux = new frmPersonas();
            //Loguin.frmCambioPassword fAux = new Loguin.frmCambioPassword();
            fAux.MdiParent = this;
            fAux.Show();
        }

        private void gestionDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PermisosSistema.PuedeGestionarUsuarios())
            {
                MessageBox.Show(Idioma.Texto("Msg_SinPermiso"));
                return;
            }
            Administrador.frmRegistrarUsuario fAux = new Administrador.frmRegistrarUsuario();
            fAux.MdiParent = this;
            fAux.Show();
        }
    }
}
