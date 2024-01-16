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

namespace CapaPresentacion.Forms
{
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Compra oCompra = new CN_Compra().ObtenerCompra(txtBusqueda.Text);

            if (oCompra.PkCompra_Id != 0)
            {
                txtnumerodocumento.Text = oCompra.NumeroDocumento;

                txtFecha.Text = oCompra.FechaRegistro;
                txtTipodocumento.Text = oCompra.TipoDocumento;
                txtUsuario.Text = oCompra.oUsuario.Nombre;
                txtCodProducto.Text = oCompra.oProveedor.Documento;
                txtRazonSocial.Text = oCompra.oProveedor.RazonSocial;

                dgvdata.Rows.Clear();

                foreach(Detalle_Compra dc in oCompra.oDetalleCompra)
                {
                    dgvdata.Rows.Add(new object[] { dc.oProducto.Nombre, dc.PrecioCompra, dc.Cantidad, dc.MontoTotal });
                }

                txtTotapagar.Text = oCompra.MontoTotal.ToString("0.00");
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipodocumento.Text = "";
            txtUsuario.Text = "";
            txtCodProducto.Text = "";

            dgvdata.Rows.Clear();
            txtTotapagar.Text = "0.00";
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {

        }

        private void frmDetalleCompra_Load(object sender, EventArgs e)
        {

        }
    }
}
