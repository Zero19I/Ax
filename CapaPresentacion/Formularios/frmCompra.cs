using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using CapaPresentacion.Modales;

namespace CapaPresentacion.Formularios
{
    public partial class frmCompra : Form
    {
        private Usuario _usuario;

        public frmCompra(Usuario ousuario = null)
        {
            InitializeComponent();
            _usuario = ousuario;
        }

        private void frmCompra_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Text = "Factura";

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProveedor.Text = "0";
            txtIdProducto.Text = "0";
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 6)
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

        private void btnBuscarProvedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProveedor())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    txtIdProveedor.Text = modal._proveedor.PkProveedor_Id.ToString();
                    txtNumDocumentoP.Text = modal._proveedor.Documento;
                    txtRazonSocialP.Text = modal._proveedor.RazonSocial;
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

                    txtprecioC.Select();
                }

                else
                {
                    txtNumDocumentoP.Select();
                }
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    dgvdata.Rows.RemoveAt(indice);
                    CalcularTotal();
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
                    txtprecioC.Select();
                }
                else
                {
                    txtCodProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtProducto.Text = "";
                }
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            decimal preciocompra = 0;
            decimal precioventa = 0;
            bool producto_existe = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("DEBE SELECCIONAR UN PRODUCTO", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtprecioC.Text, out preciocompra))
            {
                MessageBox.Show("PRECIO COMPRA - FORMATO MONEDA INCORRECTA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtprecioC.Select();
                return;
            }


            if (!decimal.TryParse(txtPecioVenta.Text, out precioventa))
            {
                MessageBox.Show("PRECIO VENTA - FORMATO MONEDA INCORRECTA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPecioVenta.Select();
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
                dgvdata.Rows.Add(new object[]
                    {
                        txtIdProducto.Text,
                        txtProducto.Text,
                        preciocompra.ToString("0.00"),
                        precioventa.ToString("0.00"),
                        txtCantidad.Value.ToString(),
                        (txtCantidad.Value * preciocompra).ToString("0.00")
                    });

                CalcularTotal();
                LimpiarProducto();
                txtCodProducto.Select();
            }
        }
        private void LimpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtCodProducto.BackColor = Color.White;
            txtProducto.Text = "";
            txtprecioC.Text = "";
            txtPecioVenta.Text = "";
            txtCantidad.Value = 1;
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

        private void txtprecioC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtprecioC.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
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

        private void txtPecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (txtPecioVenta.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
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

        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtIdProveedor.Text) == 0)
            {
                MessageBox.Show("DEBE SELECCIONAR UN PROVEEDOR", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("DEBE INGRESAR PRODUCTOS EN LA COMPRA", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_compra = new DataTable();

            detalle_compra.Columns.Add("IdProducto", typeof(int));
            detalle_compra.Columns.Add("PrecioCompra", typeof(decimal));
            detalle_compra.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_compra.Columns.Add("Cantidad", typeof(int));
            detalle_compra.Columns.Add("MontoTotal", typeof(decimal));

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                detalle_compra.Rows.Add(new object[]
                {
                    Convert.ToInt32(row.Cells["Id"].Value.ToString()),
                    row.Cells["PrecioCompra"].Value.ToString(),
                    row.Cells["PrecioVenta"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString()

                });
            }

            int idcorrelativo = new CN_Compra().ObtenerCorrelativo();
            string numerodocumento = string.Format("{0:00000}", idcorrelativo);

            Compra ocompra = new Compra()
            {
                oUsuario = new Usuario() { PkUsuario_Id = _usuario.PkUsuario_Id },
                oProveedor = new Proveedor() { PkProveedor_Id = Convert.ToInt32(txtIdProveedor.Text) },
                TipoDocumento = cboTipoDocumento.Text,
                NumeroDocumento = numerodocumento,
                MontoTotal = Convert.ToDecimal(txtTotalpagar.Text)
            };

            string mensaje = string.Empty;
            bool respuesta = new CN_Compra().Registrar(ocompra, detalle_compra, out mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("NUMERO DE COMPRA GENERADA:\n" + numerodocumento + "\n\n\n¿DESEA COPIAR AL " +
                    "PORTAPAPELES?", "MENSAJE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(numerodocumento);
                }

                txtIdProveedor.Text = "0";
                txtNumDocumentoP.Text = "";
                txtRazonSocialP.Text = "";
                dgvdata.Rows.Clear();
                CalcularTotal();
                txtTotalpagar.Text = "0.00";
            }
            else
            {
                MessageBox.Show(mensaje, "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
