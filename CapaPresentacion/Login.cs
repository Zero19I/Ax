using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //Cerrar formulario
            this.Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            List<Usuario> TEST = new CN_Usuario().Listar();
            //Creamos esta instancia para el login que valide los datos con SQL SERVER
            Usuario oUsuario = new CN_Usuario().Listar().Where(u => u.Documento == txtUsuario.Text && u.Clave == txtClave.Text).FirstOrDefault();
            // Creamos Intancias para abrir el formulario de menu principal

            //creamos una condición para entrar en caso de encuentre o no al usuario
            if(oUsuario != null)
            {
                Inicio Form = new Inicio(oUsuario);
                Form.Show();
                //ocultar formulario login
                this.Hide();

                //cuando formulario menu cierre regrese al login
                Form.FormClosing += fmrClosing;
            }

            else
            {
                //mensaje en caso de no encontrar usuario
                MessageBox.Show("Usuario no Existe","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            
        }

        //creamos evento para mostrar el formulario cerrado "Login"
        private void fmrClosing(object sender, FormClosingEventArgs e)
        {
            //Mostrar texbox Vacios
            txtClave.Text = "";
            txtUsuario.Text = "";

            //muestra el formulario Login
            this.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
