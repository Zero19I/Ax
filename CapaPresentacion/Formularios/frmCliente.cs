using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;
using System.Drawing;

namespace CapaPresentacion.Formularios
{
    public partial class frmCliente : Form
    {
        public frmCliente()
        {
            InitializeComponent();
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

        private void frmCliente_Load(object sender, EventArgs e)
        {
            cdoEstado.Items.Add(new opcionCombo() { Valor = 1, Texto = "Activo" });
            cdoEstado.Items.Add(new opcionCombo() { Valor = 0, Texto = "No Activo" });
            cdoEstado.DisplayMember = "Texto";
            cdoEstado.ValueMember = "Valor";
            cdoEstado.SelectedIndex = 0;

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


            //MOSTRAR TODOS LOS USUARIOS
            List<Cliente> lista = new CN_Cliente().Listar();

            foreach (Cliente item in lista)
            {
                dgvdata.Rows.Add
                (new object[] {
                    "",
                    item.PkCliente_Id,
                    item.Nombre,
                    item.Apellido,
                    item.Correo,
                    item.Telefono,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
                 });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Cliente obj = new Cliente()
            {
                PkCliente_Id = Convert.ToInt32(txtId.Text),
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Correo = txtCorreo.Text,
                Telefono = txtCelular.Text,
                Estado = Convert.ToInt32(((opcionCombo)cdoEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if (obj.PkCliente_Id == 0)
            {
                int idgenerado = new CN_Cliente().Registrar(obj, out mensaje);

                if (idgenerado != 0)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                        "",
                        idgenerado,
                        txtNombre.Text,
                        txtApellido.Text,
                        txtCorreo.Text,
                        txtCelular.Text,
                        ((opcionCombo)cdoEstado.SelectedItem).Valor.ToString(),
                        ((opcionCombo)cdoEstado.SelectedItem).Texto.ToString(),
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
                bool resultado = new CN_Cliente().Editar(obj, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Nombre"].Value = txtNombre.Text;
                    row.Cells["Apellido"].Value = txtApellido.Text;
                    row.Cells["Correo"].Value = txtCorreo.Text;
                    row.Cells["Telefono"].Value = txtCelular.Text;
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
            txtApellido.Text = "";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtCelular.Text = ""; ;
            cdoEstado.SelectedIndex = 0;

            txtNombre.Select();
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
                    txtNombre.Text = dgvdata.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtApellido.Text = dgvdata.Rows[indice].Cells["Apellido"].Value.ToString();
                    txtCorreo.Text = dgvdata.Rows[indice].Cells["Correo"].Value.ToString();
                    txtCelular.Text = dgvdata.Rows[indice].Cells["Telefono"].Value.ToString();

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
                if (MessageBox.Show("¿DESEA ELIMINAR EL CLIENTE?", "MENSAJE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string mensaje = string.Empty;

                    Cliente obj = new Cliente()
                    {
                        PkCliente_Id = Convert.ToInt32(txtId.Text)
                    };

                    bool respuesta = new CN_Cliente().Eliminar(obj, out mensaje);

                    if (respuesta)
                    {
                        dgvdata.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar(); /*RECUERDA*/
                    }

                    else
                    {
                        MessageBox.Show(mensaje, "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
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

        private void txtCelular_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCelular_TextChanged(object sender, EventArgs e)
        {
            if (txtCelular.Text.Length > 8)
            {
                txtCelular.Text = txtCelular.Text.Substring(0, 8);
                txtCelular.SelectionStart = 8;
            }
        }
    }
}
