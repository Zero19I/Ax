using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;

namespace CapaPresentacion.Formularios
{
    public partial class frmVenta : Form
    {
        private Usuario _usuario;
        public frmVenta(Usuario ousuario = null)
        {
            _usuario = ousuario;
            InitializeComponent();
        }

        private void frmVenta_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Text = "Factura";

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProducto.Text = "0";

            txtPagacon.Text = "";
            txtcambio.Text = "";
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtNumDocumentoP.Text = modal._cliente.Nombre;
                    txtnombrec.Text = modal._cliente.Apellido;
                    btnBuscarProducto.Select();
                }

                else
                {
                    txtNumDocumentoP.Select();
                }
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtIdProducto.Text = modal._producto.PkProducto_Id.ToString();
                    txtCodProducto.Text = modal._producto.Codigo;
                    txtProducto.Text = modal._producto.Nombre;
                    txtprecioproducto.Text = modal._producto.PrecioVenta.ToString("0.00");
                    txtStock.Text = modal._producto.Stock.ToString();

                    txtCantidad.Select();
                }

                else
                {
                    txtNumDocumentoP.Select();
                }
            }
        }

        private void txtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().Listar().Where(p => p.Codigo == txtCodProducto.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    txtCodProducto.BackColor = Color.Honeydew;
                    txtIdProducto.Text = oProducto.PkProducto_Id.ToString();
                    txtProducto.Text = oProducto.Nombre;
                    txtprecioproducto.Text = oProducto.PrecioVenta.ToString("0.00");
                    txtStock.Text = oProducto.Stock.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtCodProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtProducto.Text = "";
                    txtprecioproducto.Text = "";
                    txtStock.Text = "";
                    txtCantidad.Value = 1;
                }
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool producto_existe = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("DEBE SELECCIONAR UN PRODUCTO", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtprecioproducto.Text, out precio))
            {
                MessageBox.Show("PRECIO - FORMATO MONEDA INCORRECTA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtprecioproducto.Select();
                return;
            }


            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Value.ToString()))
            {
                MessageBox.Show("LA CANTIDAD NO PUEDE SER MAYOR AL STOCK", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            foreach (DataGridViewRow fila in dgvdata.Rows)
            {
                if (fila.Cells["Id"].Value.ToString() == txtIdProducto.Text)
                {
                    producto_existe = true;
                    break;
                }
            }

            if (!producto_existe)
            {
                bool respuesta = new CN_Venta().RestarStock(
                    Convert.ToInt32(txtIdProducto.Text),
                    Convert.ToInt32(txtCantidad.Value.ToString())
                    );

                if (respuesta == true)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                        txtIdProducto.Text,
                        txtProducto.Text,
                        precio.ToString("0.00"),
                        txtCantidad.Value.ToString(),
                        (txtCantidad.Value * precio).ToString("0.00")
                    });

                    CalcularTotal();
                    LimpiarProducto();
                    txtCodProducto.Select();
                }
            }
        }
        private void CalcularTotal()
        {
            decimal total = 0;

            if (dgvdata.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
                }
                txtTotalpagar.Text = total.ToString("0.00");
            }
        }

        private void LimpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtCodProducto.BackColor = Color.White;
            txtProducto.Text = "";
            txtprecioproducto.Text = "";
            txtStock.Text = "";
            txtCantidad.Value = 1;
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int index = e.RowIndex;

                if (index >= 0)
                {
                    bool respuesta = new CN_Venta().SumarStock(
                        Convert.ToInt32(dgvdata.Rows[index].Cells["Id"].Value.ToString()),
                        Convert.ToInt32(dgvdata.Rows[index].Cells["Cantidad"].Value.ToString()));

                    if (respuesta == true)
                    {
                        dgvdata.Rows.RemoveAt(index);
                        CalcularTotal();
                    }
                }
            }
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 5)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.delete25.Width;
                var h = Properties.Resources.delete25.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.delete25, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void txtprecioproducto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtprecioproducto.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void txtPagacon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPagacon.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }
        private void CalcularCambio()
        {
            if (txtTotalpagar.Text.Trim() == "")
            {
                MessageBox.Show("NO EXISTEN PRODUCTOS EN LA VENTA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal pagacon;
            decimal total = Convert.ToDecimal(txtTotalpagar.Text);

            if (txtPagacon.Text.Trim() == "")
            {
                txtPagacon.Text = "0";
            }

            if (decimal.TryParse(txtPagacon.Text.Trim(), out pagacon))
            {
                if (pagacon < total)
                {
                    MessageBox.Show("REVISE EL PAGO DEL CLIENTE", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                    //txtcambio.Text = "0.00";
                }

                else
                {
                    decimal cambio = pagacon - total;
                    txtcambio.Text = cambio.ToString("0.00");
                }
            }
        }

        private void txtPagacon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CalcularCambio();
            }
        }

        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            if (txtNumDocumentoP.Text == "")
            {
                MessageBox.Show("DEBE SELECCIONAR UN CLIENTE", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            else if (txtnombrec.Text == "")
            {
                MessageBox.Show("DEBE SELECCIONAR UN CLIENTE", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            else if (txtPagacon.Text == "" || txtcambio.Text == "")
            {
                MessageBox.Show("REVISE LOS DATOS DE PAGO Y CAMBIO", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            else if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("DEBE INGRESAR PRODUCTOS EN LA VENTA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            CalcularCambio();

            

            DataTable detalle_venta = new DataTable();

            detalle_venta.Columns.Add("IdProducto", typeof(int));
            detalle_venta.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("SubTotal", typeof(decimal));

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                detalle_venta.Rows.Add(new object[]
                {
                    Convert.ToInt32(row.Cells["Id"].Value.ToString()),
                    row.Cells["Precio"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString()
                });
            }

            DataTable Obtener_IdPoducto = new DataTable();
            Obtener_IdPoducto.Columns.Add("IdProducto", typeof(int));

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                Obtener_IdPoducto.Rows.Add(new object[]
                {
                  Convert.ToInt32(row.Cells["Id"].Value.ToString())
                });
            }

            int idcorrelativo = new CN_Venta().ObtenerCorrelativo();
            string numerodocumento = string.Format("{0:00000}", idcorrelativo);
            

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { PkUsuario_Id = _usuario.PkUsuario_Id },
                TipoDocumento = cboTipoDocumento.Text,
                NumeroDocumento = numerodocumento,
                DocumentoCliente = txtNumDocumentoP.Text,
                NombreCliente = txtnombrec.Text,
                MontoPago = Convert.ToDecimal(txtPagacon.Text),
                MontoCambio = Convert.ToDecimal(txtcambio.Text),
                MontoTotal = Convert.ToDecimal(txtTotalpagar.Text),
            };

            string mensaje = string.Empty;
            bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje, Obtener_IdPoducto);

            if (respuesta)
            {
                var result = MessageBox.Show("NUMERO DE VENTA GENERADA:\n" + numerodocumento + "\n\n\n¿DESEA COPIAR AL " +
                    "PORTAPAPELES?", "MENSAJE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(numerodocumento);
                }

                txtNumDocumentoP.Text = "";
                txtnombrec.Text = "";
                dgvdata.Rows.Clear();
                CalcularTotal();
                txtPagacon.Text = "";
                txtcambio.Text = "";
                txtTotalpagar.Text = "";
            }
            else
            {
                MessageBox.Show(mensaje, "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

    }
}
