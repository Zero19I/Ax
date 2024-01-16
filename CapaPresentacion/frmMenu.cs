using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Formularios;
using FontAwesome.Sharp;

namespace CapaPresentacion
{
    public partial class frmMenu : Form
    {
        private static Usuario usuarioActual;
        private static IconButton menuActivo = null;
        private static Form formularioActivo = null;

        public frmMenu(Usuario objUsuario)
        {
            usuarioActual = objUsuario;
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermiso = new CN_Permiso().Listar(usuarioActual.PkUsuario_Id);

            foreach
                (Control Buttonmenu in PanelMenu.Controls)
            {
                if (Buttonmenu is IconButton)
                {
                    bool encontrado = ListaPermiso.Any(m => m.NombreMenu == Buttonmenu.Name);

                    if (encontrado == false)
                    {
                        Buttonmenu.Visible = false;
                    }
                }
            }

            //// una ves encontrada el nombre que lo escriba en el label
            lblUsuario.Text = usuarioActual.Nombre;
            lblUsuarioApellido.Text = usuarioActual.Apellidos;
        }

        private void HideSubmenu()
        {
            if (pnlmantenedor.Visible == true)
                pnlmantenedor.Visible = false;

            if (pnlventas.Visible == true)
                pnlventas.Visible = false;

            if (pnlcompras.Visible == true)
                pnlcompras.Visible = false;

            if (pnlreportes.Visible == true)
                pnlreportes.Visible = false;
        }

        private void ShowSubMenu(Panel SubMenu)
        {
            if (SubMenu.Visible == false)
            {
                HideSubmenu();
                SubMenu.Visible = true;
            }
            else
            {
                SubMenu.Visible = false;
            }
        }

        private void AbrirFormulario(IconButton menu, Form formulario)
        {
            if (menuActivo != null)
            {
                menuActivo.BackColor = Color.FromArgb(51, 51, 51);
            }
            menu.BackColor = Color.DarkBlue;
            menuActivo = menu;

            if (formularioActivo != null) //si ya hay un formulario que lo muestre en mi contenedor
            {
                formularioActivo.Close();
            }
            formularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            Contenedor.Controls.Add(formulario);
            formulario.Show();
        }

        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconButton)sender, new frmUsuario());
        }

        private void menuExistencias_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlmantenedor);
        }

        private void SubMenuMarca_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuExistencias, new frmMarca());
            HideSubmenu();
        }

        private void submenuCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuExistencias, new frmCategoria());
            HideSubmenu();
        }

        private void submenuProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuExistencias, new frmProducto());
            HideSubmenu();
        }

        private void menuVentas_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlventas);
        }

        private void submenuRegistrarVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmVenta(usuarioActual));
            HideSubmenu();
        }

        private void submenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmDetalleVenta());
            HideSubmenu();
        }

        private void menuCompras_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlcompras);
        }

        private void submenuRegistrarCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmCompra(usuarioActual));
            HideSubmenu();
        }

        private void submenuVerDetalleCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuCompras, new frmDetalleCompra());
            HideSubmenu();
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconButton)sender, new frmCliente());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconButton)sender, new frmProveedores());
        }

        private void menuReportes_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnlreportes);
        }

        private void submenureportecompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuReportes, new frmReporteCompra());
            HideSubmenu();
        }

        private void submenureporteventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuReportes, new frmReporteVenta());
            HideSubmenu();
        }

        private void menuInfo_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconButton)sender, new FrmNegocio());

        }
    }
}
