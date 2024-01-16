using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objcd_Marca = new CD_Marca();

        public List<Marca> Listar()
        {
            return objcd_Marca.Lista();
        }
        public int Registrar(Marca obj,out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Nombre == "")
            {
                Mensaje += "ES NECESARIO EL NOMBRE DE LA MARCA\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }

            else
            {
                return objcd_Marca.Registrar(obj, out Mensaje);
            }
        }
        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Nombre == "")
            {
                Mensaje += "ES NECESARIO EL NOMBRE DE LA MARCA\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }

            else
            {
                return objcd_Marca.Editar(obj, out Mensaje);
            }
        }
        public bool Eliminar(Marca obj, out string Mensaje)
        {
            return objcd_Marca.Eliminar(obj, out Mensaje);
        }
    }
}
