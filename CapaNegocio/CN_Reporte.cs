using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objcd_Reporte = new CD_Reporte();

        public List<ReporteCompra> Compra(string fechainicio, string fechafin, int idproveedor)
        {
            return objcd_Reporte.Compra(fechainicio, fechafin, idproveedor);
        }

        public List<ReporteVenta> Compra(string fechainico, string fechafin)
        {
            return objcd_Reporte.Venta(fechainico, fechafin);
        }

    }
}
