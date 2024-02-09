using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "USUARIO") //CUANDO PRESIONAMOS EL LABEL Y TIENE POR DEFECTO COMO TEXTO USUARIO
            {
                txtuser.Text = "";   //QUE ME LO LIMPIE Y QUE PUEDA ESCRIBIR OTRA COSA
                txtuser.ForeColor = Color.DimGray;     // SOLO EL COLOR
            }
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtuser.Text == "")  // SI EL LABEL ESTA VACIO Y LO DESELECCIONO QUE LO DEJE COMO ESTABA 
            {
                txtuser.Text = "USUARIO";    // OSEA EL TEXTO USUARIO QUE TENIA POR DEFECTO
                txtuser.ForeColor = Color.DimGray;   // SOLO EL COLOR
            }
        }

        private void txtpassword_Enter(object sender, EventArgs e)
        {
            if (txtpassword.Text == "CONTRASEÑA")   // CUANDO PRESIONAMOS EL LABEL Y TIENE POR DEFECTO COMO TEXTO CONTRASEÑA
            {
                txtpassword.Text = "";    //QUE ME LO LIMPIE Y QUE PUEDA ESCRIBIR OTRA COSA
                txtpassword.ForeColor = Color.DimGray;  //SOLO EL COLOR
                txtpassword.UseSystemPasswordChar = true; // CODIGO PARA EL TEXTO QUE VAYA ESCRIBIR SE OCULTE
            }
        }

        private void txtpassword_Leave(object sender, EventArgs e)
        {
            if (txtpassword.Text == "")  // SI EL LABEL ESTA VACIO Y LO DESELECCIONO QUE LO DEJE COMO ESTABA
            {
                txtpassword.Text = "CONTRASEÑA";  // OSEA EL TEXTO CONRASEÑA QUE TENIA POR DEFECTO
                txtpassword.ForeColor = Color.DimGray;    //SOLO EL COLOR
                txtpassword.UseSystemPasswordChar = false;    // Y COMO ME VA A DEJAR EL TEXTO QUE TENIA QUE ME DESACTIVE
                                                                // EL MODO CONTRASEÑA PORQUE SI NO EL TEXTO CONTRASEÑA ESTARIA OCULTO
            }
        }
    }
}
