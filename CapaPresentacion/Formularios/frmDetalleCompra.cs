using System;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text;

namespace CapaPresentacion.Formularios
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
                txtApellidosVe.Text = oCompra.oUsuario.Apellidos;

                txtdoccliente.Text = oCompra.oProveedor.Documento;
                txtNombreCliente.Text = oCompra.oProveedor.RazonSocial;

                dgvdata.Rows.Clear();
                foreach (Detalle_Compra dc in oCompra.oDetalleCompra)
                {
                    dgvdata.Rows.Add(new object[] { dc.oProducto.Nombre, dc.PrecioCompra, dc.Cantidad, dc.MontoTotal });
                }

                txtmontototal.Text = oCompra.MontoTotal.ToString("0.00");

            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipodocumento.Text = "";
            txtUsuario.Text = "";
            txtdoccliente.Text = "";
            txtNombreCliente.Text = "";

            dgvdata.Rows.Clear();
            txtmontototal.Text = "0.00";
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            if (txtTipodocumento.Text == "")
            {
                MessageBox.Show("NO SE ENCUNTRARON RESULTADOS", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string Texto_Html = Properties.Resources.PlantillaCompra.ToString();
            Negocio odatos = new CN_Negocio().ObtenerDatos();

            Texto_Html = Texto_Html.Replace("@nombrenegocio", odatos.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", odatos.RUC);
            Texto_Html = Texto_Html.Replace("@direcnegocio", odatos.Direccion);

            Texto_Html = Texto_Html.Replace("@tipodocumento", txtTipodocumento.Text);
            Texto_Html = Texto_Html.Replace("@numerodocumento", txtnumerodocumento.Text);

            Texto_Html = Texto_Html.Replace("@doccliente", txtdoccliente.Text);
            Texto_Html = Texto_Html.Replace("@nombrecliente", txtNombreCliente.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro", txtFecha.Text);
            Texto_Html = Texto_Html.Replace("@usuarioregistro", txtUsuario.Text);

            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Precio"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                filas += "</tr>";
            }

            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtmontototal.Text);

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Venta_{0}.pdf", txtnumerodocumento.Text);
            saveFile.Filter = "Pdf Files|*.pdf";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();

                    bool obtenido = true;
                    byte[] byteImagen = new CN_Negocio().ObtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(byteImagen);
                        image.ScaleToFit(60, 60);
                        image.Alignment = iTextSharp.text.Image.UNDERLYING;
                        image.SetAbsolutePosition(pdfdoc.Left, pdfdoc.GetTop(51));
                        pdfdoc.Add(image);
                    }

                    using (StringReader sr = new StringReader(Texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfdoc, sr);
                    }

                    pdfdoc.Close();
                    stream.Close();
                    MessageBox.Show("DOCUMENTO GENERADO", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
