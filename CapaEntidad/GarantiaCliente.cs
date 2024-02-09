using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class GarantiaCliente
    {
        public int PkDetalleVenta_Id { get; set; }
        public int PkProducto_Id { get; set; }
        public string NumeroDocumento { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string FechaInicio { get; set; }
        public string FechaLimite { get; set; }
        public bool Estado { get; set; }
    }
}
