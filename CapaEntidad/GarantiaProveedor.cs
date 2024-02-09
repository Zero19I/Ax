using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class GarantiaProveedor
    {
        public string Proveedor { get; set; }
        public decimal PrecioCompra { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public int idproducto { get; set; }
    }
}
