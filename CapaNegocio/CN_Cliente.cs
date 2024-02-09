using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Cliente

    {
        private CD_Cliente objcd_Cliente = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return objcd_Cliente.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (obj.Nombre == "")
            {
                Mensaje += "ES NECESARIO UN NOMBRE\n";
            }

            if (obj.Apellido == "")
            {
                Mensaje += "ES NECESARIO UN APELLIDO\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "ES NECESARIO EL CORREO\n";
            }

            if (obj.Telefono == "")
            {
                Mensaje += "ES NECESARIO EL CELULAR\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }

            else
            {
                return objcd_Cliente.Registrar(obj, out Mensaje);
            }

        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Nombre == "")
            {
                Mensaje += "ES NECESARIO UN NOMBRE\n";
            }

            if (obj.Apellido == "")
            {
                Mensaje += "ES NECESARIO UN APELLIDO\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "ES NECESARIO EL CORREO\n";
            }

            if (obj.Telefono == "")
            {
                Mensaje += "ES NECESARIO EL CELULAR\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }

            else
            {
                return objcd_Cliente.Editar(obj, out Mensaje);
            }

        }

        public bool Eliminar(Cliente obj, out string Mensaje)
        {
            return objcd_Cliente.Eliminar(obj, out Mensaje);
        }
    }
}
