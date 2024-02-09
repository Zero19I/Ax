using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel;

namespace CapaPresentacion.Formularios
{
    public partial class frmReporteCompra : Form
    {
        public frmReporteCompra()
        {
            InitializeComponent();

            txtfechainicio.Value = DateTime.Today.AddDays(-7);
            txtfechafin.Value = DateTime.Now;
        }

        private void frmReporteCompra_Load(object sender, EventArgs e)
        {
            List<Proveedor> lista = new CN_Proveedor().Listar();

            cboproveedor.Items.Add(new opcionCombo() { Valor = 0, Texto = "TODOS" });

            foreach (Proveedor item in lista)
            {
                cboproveedor.Items.Add(new opcionCombo() { Valor = item.PkProveedor_Id, Texto = item.RazonSocial });
            }

            cboproveedor.DisplayMember = "Texto";
            cboproveedor.ValueMember = "Valor";
            cboproveedor.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                cdoBusqueda.Items.Add(new opcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
            }

            cdoBusqueda.DisplayMember = "Texto";
            cdoBusqueda.ValueMember = "Valor";
            cdoBusqueda.SelectedIndex = 0;

            carga();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            carga();
        }

        private void carga()
        {
            int idproveedor = Convert.ToInt32(((opcionCombo)cboproveedor.SelectedItem).Valor.ToString());

            List<ReporteCompra> lista = new List<ReporteCompra>();

            lista = new CN_Reporte().Compra
                (
                txtfechainicio.Value,
                txtfechafin.Value,
                idproveedor
                );

            dgvdata.Rows.Clear();

            foreach (ReporteCompra rc in lista)
            {
                dgvdata.Rows.Add(new object[]
                {
                    rc.FechaRegistro,
                    rc.NumeroDocumento,
                    rc.UsuarioRegistro,
                    rc.Apellidos,
                    rc.RazonSocial,
                    rc.CodigoProducto,
                    rc.NombreProducto,
                    rc.Categoria,
                    rc.Marca,
                    rc.PrecioCompra,
                    rc.PrecioVenta,
                    rc.Cantidad,
                    rc.SubTotal
                });
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("NO HAY DATOS PARA EXPORTAR", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            else
            {
                DataTable dataTable = new DataTable();

                foreach (DataGridViewColumn column in dgvdata.Columns)
                {
                    dataTable.Columns.Add(column.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Visible)
                        dataTable.Rows.Add(new object[]
                            {
                                row.Cells[0].Value.ToString(),
                                row.Cells[1].Value.ToString(),
                                row.Cells[2].Value.ToString(),
                                row.Cells[3].Value.ToString(),
                                row.Cells[4].Value.ToString(),
                                row.Cells[5].Value.ToString(),
                                row.Cells[6].Value.ToString(),
                                row.Cells[7].Value.ToString(),
                                row.Cells[8].Value.ToString(),
                                row.Cells[9].Value.ToString(),
                                row.Cells[10].Value.ToString(),
                                row.Cells[11].Value.ToString(),
                                row.Cells[12].Value.ToString(),
                            });
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = string.Format("ReporteCompra_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                saveFile.Filter = "Excel Files | *.xlsx";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dataTable, "Informe");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(saveFile.FileName);
                        MessageBox.Show("REPORTE GENERADO", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    catch
                    {
                        MessageBox.Show("ERROR AL GENERAR REPORTE", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnBuscarF_Click(object sender, EventArgs e)
        {
            string columnafiltro = ((opcionCombo)cdoBusqueda.SelectedItem).Valor.ToString();

            if (dgvdata.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Cells[columnafiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }

                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }
    }
}
