using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;
using System.Drawing;

namespace CapaPresentacion.Formularios
{
    public partial class frmGarantiaCliente : Form
    {
        public frmGarantiaCliente()
        {
            InitializeComponent();
        }

        private void frmGarantiaCliente_Load(object sender, EventArgs e)
        {
            List<GarantiaCliente> Lista = new CN_Garantia().GarantiaCliente();

            foreach (GarantiaCliente item in Lista)
            {
                dgvdata.Rows.Add
                 (new object[]
                 {
                     "",
                     item.PkDetalleVenta_Id,
                     item.PkProducto_Id,
                     item.NumeroDocumento,
                     item.Producto,
                     item.Cantidad,
                     item.FechaInicio,
                     item.FechaLimite,
                     item.Estado == true ? 1 : 0,
                     item.Estado == true ? "Vigente" : "Expiro"
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
                    txtIdDetalleVenta.Text = dgvdata.Rows[indice].Cells["IdDV"].Value.ToString();
                    txtIdProducto.Text = dgvdata.Rows[indice].Cells["IdProducto"].Value.ToString();
                    txtIdVenta.Text = dgvdata.Rows[indice].Cells["NDocumento"].Value.ToString();
                    txtNombreProducto.Text = dgvdata.Rows[indice].Cells["Producto"].Value.ToString();
                    txtCantidad.Text = dgvdata.Rows[indice].Cells["Cantidad"].Value.ToString();
                    txtInicioFecha.Text = dgvdata.Rows[indice].Cells["FechaInicio"].Value.ToString();
                    txtFinFecha.Text = dgvdata.Rows[indice].Cells["FechaVencimiento"].Value.ToString();
                    txtEstado.Text = dgvdata.Rows[indice].Cells["EstadoValor"].Value.ToString();
                    txtEstadoTexto.Text = dgvdata.Rows[indice].Cells["Estado"].Value.ToString();

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
            string mensaje = string.Empty;

            if (txtEstado.Text == "")
            {
                MessageBox.Show("SELECCIONE", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else 
            { 
                if (txtEstado.Text == "0")
                {
                    MessageBox.Show("LA GARANTIA YA ESTA VALIDADA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    GarantiaCliente obj = new GarantiaCliente()
                    {
                        PkDetalleVenta_Id = Convert.ToInt32(txtIdDetalleVenta.Text),
                        PkProducto_Id = Convert.ToInt32(txtIdProducto.Text),
                        Cantidad = Convert.ToInt32(txtCantidad.Text),
                        NumeroDocumento = txtIdVenta.Text
                    };

                    bool resultado = new CN_Garantia().Registrar(obj);

                    if (resultado)
                    {
                        MessageBox.Show(mensaje, "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else
                    {

                        MessageBox.Show("SE REALIZO LA TRANSACCIÓN", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtIndice.Text)];
                        row.Cells["EstadoValor"].Value = 0;
                        row.Cells["Estado"].Value = "Expiro";
                        Limpiar();
                    }
                }
            }
        }

        private void Limpiar()
        {
            txtCantidad.Text = "";
            txtEstado.Text = "";
            txtEstadoTexto.Text = "";
            txtFinFecha.Text = "";
            txtIdDetalleVenta.Text = "";
            txtIdProducto.Text = "";
            txtIdVenta.Text = "";
            txtIndice.Text = "";
            txtInicioFecha.Text = "";
            txtNombreProducto.Text = "";
        }
    }
}
