using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objcd_Usuario = new CD_Usuario();

        public List<Usuario> Listar()
        {
            return objcd_Usuario.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if(obj.Documento == "")
            {
                Mensaje += "ES NECESARIO UN USUARIO\n";
            }

            if(obj.Nombre == "")
            {
                Mensaje += "ES NECESARIO EL NOMBRE\n";
            }

            if (obj.Apellidos == "")
            {
                Mensaje += "ES NECESARIO EL APELLIDO\n";
            }

            if (obj.Telefono == "")
            {
                Mensaje += "ES NECESARIO UN NUMERO DE TELEFONO\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "ES NECESARIO EL CORREO\n";
            }

            if (obj.Clave == "")
            {
                Mensaje += "ES NECESARIO LA CLAVE\n";
            }

            if(Mensaje != string.Empty)
            {
                return 0;
            }

            else
            {
                return objcd_Usuario.Registrar(obj, out Mensaje);
            }

        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "ES NECESARIO UN USUARIO\n";
            }

            if (obj.Nombre == "")
            {
                Mensaje += "ES NECESARIO EL NOMBRE\n";
            }

            if (obj.Apellidos == "")
            {
                Mensaje += "ES NECESARIO EL APELLIDO\n";
            }

            if (obj.Telefono == "")
            {
                Mensaje += "ES NECESARIO UN NUMERO DE TELEFONO\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "ES NECESARIO EL CORREO\n";
            }

            if (obj.Clave == "")
            {
                Mensaje += "ES NECESARIO LA CLAVE\n";
            }


            if (Mensaje != string.Empty)
            {
                return false;
            }

            else
            {
                return objcd_Usuario.Editar(obj, out Mensaje);
            }

        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            return objcd_Usuario.Eliminar(obj, out Mensaje);
        }
    }
}
