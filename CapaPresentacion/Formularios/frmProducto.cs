using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using System.Drawing;

namespace CapaPresentacion.Formularios
{
    public partial class frmProducto : Form
    {
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            cdoEstado.Items.Add(new opcionCombo() { Valor = 1, Texto = "Activo" });
            cdoEstado.Items.Add(new opcionCombo() { Valor = 0, Texto = "No Activo" });
            cdoEstado.DisplayMember = "Texto";
            cdoEstado.ValueMember = "Valor";
            cdoEstado.SelectedIndex = 0;

            List<Categoria> listacategoria = new CN_Categoria().Listar();

            foreach (Categoria item in listacategoria)
            {
                cdoCategoria.Items.Add(new opcionCombo() { Valor = item.PkCategoria, Texto = item.Descripcion });
            }
            cdoCategoria.DisplayMember = "Texto";
            cdoCategoria.ValueMember = "Valor";
            cdoCategoria.SelectedIndex = 0;

            List<Marca> listamarca = new CN_Marca().Listar();

            foreach (Marca item in listamarca)
            {
                cboMarca.Items.Add(new opcionCombo() { Valor = item.Id, Texto = item.Nombre });
            }
            cboMarca.DisplayMember = "Texto";
            cboMarca.ValueMember = "Valor";
            cboMarca.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cdoBusqueda.Items.Add(new opcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cdoBusqueda.DisplayMember = "Texto";
            cdoBusqueda.ValueMember = "Valor";
            cdoBusqueda.SelectedIndex = 0;


            //MOSTRAR LOS PRODUCTOS
            List<Producto> lista = new CN_Producto().Listar();

            foreach (Producto item in lista)
            {
                dgvdata.Rows.Add
                (new object[]{
                    "",
                    item.PkProducto_Id,
                    item.Codigo,
                    item.Nombre,
                    item.Descripcion,
                    item.oCategoria.PkCategoria,
                    item.oCategoria.Descripcion,
                    item.oMarca.Id,
                    item.oMarca.Nombre,
                    item.Stock,
                    item.PrecioCompra,
                    item.PrecioVenta,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Producto objproducto = new Producto()
            {
                PkProducto_Id = Convert.ToInt32(txtId.Text),
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                oCategoria = new Categoria() { PkCategoria = Convert.ToInt32(((opcionCombo)cdoCategoria.SelectedItem).Valor) },
                oMarca = new Marca() { Id = Convert.ToInt32(((opcionCombo)cboMarca.SelectedItem).Valor) },
                Estado = Convert.ToInt32(((opcionCombo)cdoEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if (objproducto.PkProducto_Id == 0)
            {
                int idgenerado = new CN_Producto().Registrar(objproducto, out mensaje);

                if (idgenerado != 0)
                {

                    dgvdata.Rows.Add(new object[]
                    {
                        "",
                        idgenerado,
                        txtCodigo.Text,
                        txtNombre.Text,
                        txtDescripcion.Text,
                        ((opcionCombo)cdoCategoria.SelectedItem).Valor.ToString(),
                        ((opcionCombo)cdoCategoria.SelectedItem).Texto.ToString(),
                        ((opcionCombo)cboMarca.SelectedItem).Valor.ToString(),
                        ((opcionCombo)cboMarca.SelectedItem).Texto.ToString(),
                        "0",
                        "0.00",
                        "0.00",
                        ((opcionCombo)cdoEstado.SelectedItem).Valor.ToString(),
                        ((opcionCombo)cdoEstado.SelectedItem).Texto.ToString()

                    });

                    Limpiar();
                }

                else
                {
                    MessageBox.Show(mensaje);
                }
            }

            else
            {
                bool resultado = new CN_Producto().Editar(objproducto, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Codigo"].Value = txtCodigo.Text;
                    row.Cells["Nombre"].Value = txtNombre.Text;
                    row.Cells["Descripcion"].Value = txtDescripcion.Text;

                    row.Cells["IdCategoria"].Value = ((opcionCombo)cdoCategoria.SelectedItem).Valor.ToString();
                    row.Cells["Categoria"].Value = ((opcionCombo)cdoCategoria.SelectedItem).Texto.ToString();

                    row.Cells["IdMarca"].Value = ((opcionCombo)cboMarca.SelectedItem).Valor.ToString();
                    row.Cells["Marca"].Value = ((opcionCombo)cboMarca.SelectedItem).Texto.ToString();

                    row.Cells["EstadoValor"].Value = ((opcionCombo)cdoEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((opcionCombo)cdoEstado.SelectedItem).Texto.ToString();

                    Limpiar();
                }

                else
                {
                    MessageBox.Show(mensaje);
                }
            }
        }

        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            cdoEstado.SelectedIndex = 0;
            cdoCategoria.SelectedIndex = 0;

            txtCodigo.Select();
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

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvdata.Rows[indice].Cells["Id"].Value.ToString();
                    txtCodigo.Text = dgvdata.Rows[indice].Cells["Codigo"].Value.ToString();
                    txtNombre.Text = dgvdata.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtDescripcion.Text = dgvdata.Rows[indice].Cells["Descripcion"].Value.ToString();

                    foreach (opcionCombo oc in cdoCategoria.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdCategoria"].Value))
                        {
                            int indice_combo = cdoCategoria.Items.IndexOf(oc);
                            cdoCategoria.SelectedIndex = indice_combo;
                            break;
                        }
                    }

                    foreach (opcionCombo oc in cboMarca.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdMarca"].Value))
                        {
                            int indice_combo = cboMarca.Items.IndexOf(oc);
                            cboMarca.SelectedIndex = indice_combo;
                            break;
                        }
                    }

                    foreach (opcionCombo oc in cdoEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cdoEstado.Items.IndexOf(oc);
                            cdoEstado.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿DESEA ELIMINAR EL PRODUCTO?", "MENSAJE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string mensaje = string.Empty;

                    Producto objproducto = new Producto()
                    {
                        PkProducto_Id = Convert.ToInt32(txtId.Text)
                    };

                    bool respuesta = new CN_Producto().Eliminar(objproducto, out mensaje);

                    if (respuesta)
                    {
                        dgvdata.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    }

                    else
                    {
                        MessageBox.Show(mensaje, "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show(" NO HAY DATOS PARA EXPORTAR", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            else
            {
                DataTable dataTable = new DataTable();

                foreach (DataGridViewColumn column in dgvdata.Columns)
                {
                    if (column.HeaderText != "" && column.Visible)
                        dataTable.Columns.Add(column.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in dgvdata.Rows)
                {
                    if (row.Visible)
                        dataTable.Rows.Add(new object[]
                            {
                                row.Cells[2].Value.ToString(),
                                row.Cells[3].Value.ToString(),
                                row.Cells[4].Value.ToString(),
                                row.Cells[6].Value.ToString(),
                                row.Cells[8].Value.ToString(),
                                row.Cells[9].Value.ToString(),
                                row.Cells[10].Value.ToString(),
                                row.Cells[11].Value.ToString(),
                                row.Cells[13].Value.ToString(),
                            });
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = string.Format("ReporteProducto_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
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
    }
}
