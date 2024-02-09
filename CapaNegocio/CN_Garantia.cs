using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Garantia
    {
        private CD_Garantia objcd_Garantia = new CD_Garantia();

        public List<GarantiaCliente> GarantiaCliente()
        {
            return objcd_Garantia.Listar();
        }

        public bool Registrar(GarantiaCliente obj)
        {
            return objcd_Garantia.RegistrarProductoRIP(obj);
        }

        public List<GarantiaProveedor> GarantiaProveedor()
        {
            return objcd_Garantia.ListarGP();
        }
        public bool RealizarCambioProveedor(GarantiaProveedor obj)
        {
            return objcd_Garantia.RealizarCambioProveedor(obj);
        }
    }
}
