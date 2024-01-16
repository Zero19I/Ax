using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Forms;
using FontAwesome.Sharp;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {
        //variables a utlizar 
        private static Usuario usuarioActual;
        private static IconMenuItem menuActivo = null;
        private static Form formularioActivo = null;

        //private static IconButton iconb = null; //borrar

        //hacemos que busque el nombre en la BD para escribirle en label
        public Inicio( Usuario objUsuario)
        {
            usuarioActual = objUsuario;
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermiso = new CN_Permiso().Listar(usuarioActual.PkUsuario_Id);

            foreach (IconMenuItem iconMenu in menu.Items)
            {
                bool encontrado = ListaPermiso.Any(m => m.NombreMenu == iconMenu.Name);

                if(encontrado == false)
               {
                    iconMenu.Visible = false;
                }
            }

            // una ves encontrada el nombre que lo escriba en el label
            lblUsuario.Text = usuarioActual.Nombre;
        }

        //Evento para mostrar formularios
        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {
            if( menuActivo != null)
            {
                menuActivo.BackColor = Color.White;
            }
            menu.BackColor = Color.Silver;
            menuActivo = menu;

            if (formularioActivo != null) //si ya hay un formulario que lo muestre en mi contenedor
            {
                formularioActivo.Close();
            }
            formularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.SteelBlue;

            contenedor.Controls.Add(formulario);
            formulario.Show();
        }

        //private void AbrirFormulariobotton(IconButton menu, Form formulario) //borrar
        //{
        //    if (iconb != null)
        //    {
        //        iconb.BackColor = Color.White;
        //    }
        //    menu.BackColor = Color.Silver;
        //    iconb = menu;

        //    if (formularioActivo != null) //si ya hay un formulario que lo muestre en mi contenedor
        //    {
        //        formularioActivo.Close();
        //    }
        //    formularioActivo = formulario;
        //    formulario.TopLevel = false;
        //    formulario.FormBorderStyle = FormBorderStyle.None;
        //    formulario.Dock = DockStyle.Fill;
        //    formulario.BackColor = Color.SteelBlue;

        //    contenedor.Controls.Add(formulario);
        //    formulario.Show();
        //}

        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmUsuario());
        }

        private void submenuCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuExistencias, new frmCategoria());
        }

        private void submenuProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuExistencias, new frmProducto());
        }

        private void submenuRegistrarVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmVenta(usuarioActual));
        }

        private void submenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmDetalleVenta());
        }

        private void submenuRegistrarCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuCompras, new frmCompra(usuarioActual));
        }

        private void submenuVerDetalleCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuCompras, new frmDetalleCompra());
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmCliente());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmProveedores());
        }

        private void menuReportes_Click(object sender, EventArgs e)
        {
        }

        private void menuInfo_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmNegocio());
        }

        private void submenureportecmpra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuReportes,new frmReporteCompra());
        }

        private void submenureporteventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuReportes, new frmReporteVenta());
        }

        private void SubMenuMarca_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuExistencias, new frmMarca());
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void menuExistencias_Click(object sender, EventArgs e)
        {

        }

        private void menuVentas_Click(object sender, EventArgs e)
        {

        }

        //private void iconButton1_Click(object sender, EventArgs e) //borrar
        //{
        //    AbrirFormulariobotton((IconButton)sender, new frmUsuario());
        //}
    }
}
