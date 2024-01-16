using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            List<Usuario> TEST = new CN_Usuario().Listar();
            //Creamos esta instancia para el login que valide los datos con SQL SERVER
            Usuario oUsuario = new CN_Usuario().Listar().Where(u => u.Documento == txtuser.Text && u.Clave == txtpassword.Text).FirstOrDefault();
            // Creamos Intancias para abrir el formulario de menu principal

            //creamos una condición para entrar en caso de encuentre o no al usuario
            if (oUsuario != null)
            {
                frmMenu Form = new frmMenu(oUsuario);
                Form.Show();
                //ocultar formulario login
                this.Hide();

                //cuando formulario menu cierre regrese al login
                Form.FormClosing += fmrClosing;
            }

            else
            {
                //mensaje en caso de no encontrar usuario
                MessageBox.Show("USUARIO NO EXISTE", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void fmrClosing(object sender, FormClosingEventArgs e)
        {
            //Mostrar texbox Vacios
            txtpassword.Text = "";
            txtuser.Text = "";

            //muestra el formulario Login
            this.Show();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();  //Cerrar formulario
        }

    }
}
