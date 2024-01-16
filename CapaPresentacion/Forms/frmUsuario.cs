using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion.Forms
{
    public partial class frmUsuario : Form
    {
        public frmUsuario()
        {
            InitializeComponent();
        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            cdoEstado.Items.Add(new opcionCombo() { Valor = 1, Texto = "Activo" });
            cdoEstado.Items.Add(new opcionCombo() { Valor = 0, Texto = "No Activo" });
            cdoEstado.DisplayMember = "Texto";
            cdoEstado.ValueMember = "Valor";
            cdoEstado.SelectedIndex = 0;

            List<Rol> listaRol = new CN_Rol().Listar();

            foreach (Rol item in listaRol)
            {
                cdoRol.Items.Add(new opcionCombo() { Valor = item.PkRol_Id, Texto = item.Descripcion });
            }
            cdoRol.DisplayMember = "Texto";
            cdoRol.ValueMember = "Valor";
            cdoRol.SelectedIndex = 0;

            foreach(DataGridViewColumn columna  in dgvdata.Columns)
            {
                if(columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cdoBusqueda.Items.Add(new opcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cdoBusqueda.DisplayMember = "Texto";
            cdoBusqueda.ValueMember = "Valor";
            cdoBusqueda.SelectedIndex = 0;


            //MOSTRAR TODOS LOS USUARIOS
            List<Usuario> listaUsuario = new CN_Usuario().Listar();

            foreach (Usuario item in listaUsuario)
            {
                dgvdata.Rows.Add
                (new object[] {"",
                    item.PkUsuario_Id,
                    item.Documento,
                    item.Nombre,
                    item.Apellidos,
                    item.Correo,
                    item.Telefono,
                    item.Clave,
                    item.oRol.PkRol_Id,
                    item.oRol.Descripcion,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
                 });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Usuario objusuario = new Usuario()
            {
                PkUsuario_Id = Convert.ToInt32(txtId.Text),
                Documento = txtUsuario.Text,
                Nombre = txtNombre.Text,
                Apellidos = txtApellido.Text,
                Correo = txtCorreo.Text,
                Telefono = txtCelular.Text,
                Clave = txtClave.Text,
                oRol = new Rol() { PkRol_Id = Convert.ToInt32(((opcionCombo)cdoRol.SelectedItem).Valor)},
                Estado = Convert.ToInt32(((opcionCombo)cdoEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if(objusuario.PkUsuario_Id == 0)
            {
                int idusuariogenerado = new CN_Usuario().Registrar(objusuario, out mensaje);

                if (idusuariogenerado != 0)
                {

                    dgvdata.Rows.Add(new object[]
                    {   "",
                        idusuariogenerado,
                        txtUsuario.Text,
                        txtNombre.Text,
                        txtApellido.Text,
                        txtCorreo.Text,
                        txtCelular.Text,
                        txtClave.Text,
                        ((opcionCombo)cdoRol.SelectedItem).Valor.ToString(),
                        ((opcionCombo)cdoRol.SelectedItem).Texto.ToString(),
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
                bool resultado = new CN_Usuario().Editar(objusuario, out mensaje);

                if(resultado)
                {
                    DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Usuario"].Value = txtUsuario.Text;
                    row.Cells["Nombre"].Value = txtNombre.Text;
                    row.Cells["Apellidos"].Value = txtApellido.Text;
                    row.Cells["Correo"].Value = txtCorreo.Text;
                    row.Cells["Celular"].Value = txtCelular.Text;
                    row.Cells["Clave"].Value = txtClave.Text;
                    row.Cells["IdRol"].Value = ((opcionCombo)cdoRol.SelectedItem).Valor.ToString();
                    row.Cells["Rol"].Value = ((opcionCombo)cdoRol.SelectedItem).Texto.ToString();
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
            txtUsuario.Text = "";
            txtNombre.Text = ""; 
            txtApellido.Text = "";
            txtCorreo.Text = "";
            txtCelular.Text = "";
            txtClave.Text = "";
            txtConfirmarClave.Text = "";
            cdoEstado.SelectedIndex = 0;
            cdoRol.SelectedIndex = 0;

            txtUsuario.Select();
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if(e.RowIndex < 0)
            //    return;

            //if(e.ColumnIndex == 0)
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

            //    var w = Properties.Resources.Check.Width;
            //    var h = Properties.Resources.Check.Height;
            //    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
            //    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

            //    e.Graphics.DrawImage(Properties.Resources.Check, new Rectangle(x,y,w,h));
            //    e.Handled = true;
            //}
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvdata.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if(indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvdata.Rows[indice].Cells["Id"].Value.ToString();
                    txtUsuario.Text = dgvdata.Rows[indice].Cells["Usuario"].Value.ToString();
                    txtNombre.Text = dgvdata.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtApellido.Text = dgvdata.Rows[indice].Cells["Apellidos"].Value.ToString();
                    txtCorreo.Text = dgvdata.Rows[indice].Cells["Correo"].Value.ToString();
                    txtCelular.Text = dgvdata.Rows[indice].Cells["Celular"].Value.ToString();
                    txtClave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();
                    txtConfirmarClave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();

                    foreach(opcionCombo oc in cdoRol.Items)
                    {
                        if(Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["IdRol"].Value))
                        {
                            int indice_combo = cdoRol.Items.IndexOf(oc);
                            cdoRol.SelectedIndex = indice_combo;
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
            if(Convert.ToInt32(txtId.Text) != 0)
            {
                if(MessageBox.Show("¿DESEA ELIMINAR EL USUARIO?","MENSAJE",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string mensaje = string.Empty;

                    Usuario objusuario = new Usuario()
                    {
                        PkUsuario_Id = Convert.ToInt32(txtId.Text)
                    };
        
                    bool respuesta = new CN_Usuario().Eliminar(objusuario, out mensaje);

                    if(respuesta)
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

            if(dgvdata.Rows.Count > 0)
            {
                foreach(DataGridViewRow row in dgvdata.Rows)
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
            foreach(DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnEditarLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
