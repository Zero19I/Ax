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

namespace CapaPresentacion.Formularios
{
    public partial class frmGarantiaProveedor : Form
    {
        public frmGarantiaProveedor()
        {
            InitializeComponent();
        }

        private void frmGarantiaProveedor_Load(object sender, EventArgs e)
        {
            List<GarantiaProveedor> Lista = new CN_Garantia().GarantiaProveedor();

            foreach (GarantiaProveedor item in Lista)
            {
                dgvdata.Rows.Add
                 (new object[]
                 {
                     "",
                     item.Proveedor,
                     item.PrecioCompra,
                     item.Producto,
                     item.Cantidad,
                     item.idproducto
                 });
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtProveedor.Text = dgvdata.Rows[indice].Cells["Proveedor"].Value.ToString();
                    txtProducto.Text = dgvdata.Rows[indice].Cells["Producto"].Value.ToString();
                    txtPrecio.Text = dgvdata.Rows[indice].Cells["Precio"].Value.ToString();
                    txtCantidad.Text = dgvdata.Rows[indice].Cells["Cantidad"].Value.ToString();
                    txtIdProducto.Text = dgvdata.Rows[indice].Cells["IdProducto"].Value.ToString();
                }
            }
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void btnCambio_Click(object sender, EventArgs e)
        {

            if (txtIdProducto.Text == "")
            {
                MessageBox.Show("SELECCIONE", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (txtCantidad.Text == "0")
                {
                    MessageBox.Show("LA GARANTIA YA ESTA VALIDADA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    GarantiaProveedor obj = new GarantiaProveedor()
                    {
                        idproducto = Convert.ToInt32(txtIdProducto.Text),
                        Cantidad = Convert.ToInt32(txtCantidad.Text)
                    };

                    bool resultado = new CN_Garantia().RealizarCambioProveedor(obj);

                    if (resultado)
                    {
                        MessageBox.Show("bien", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else
                    {

                        MessageBox.Show("SE REALIZO LA TRANSACCION", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtIndice.Text)];
                        row.Cells["Cantidad"].Value = "0";
                        Limpiar();
                    }
                }
            }
        }

        private void Limpiar()
        {
            txtCantidad.Text = "";
            txtIdProducto.Text = "";
            txtIndice.Text = "";
            txtPrecio.Text = "";
            txtProducto.Text = "";
            txtProveedor.Text = "";
        }
    }
}
