using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Dashboard
    {
        //public List<Dashboard> GetNumberItems()
        //{
        //    List<Dashboard> lista = new List<Dashboard>();

        //    using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
        //    {
        //        try
        //        {
        //            oConexion.Open();

        //            using (var command = new SqlCommand())
        //            {
        //                command.Connection = oConexion;

        //                command.CommandText = "SELECT COUNT(PkCliente_Id) FROM Tbl_Cliente";
        //                numorder = (int)command.ExecuteScalar();

        //                command.CommandText = "SELECT COUNT(PkProveedor_Id) FROM Tbl_Proveedor";
        //                NumSuppliers = (int)command.ExecuteScalar();

        //                command.CommandText = "SELECT COUNT(PkProducto_Id) FROM tbl_Producto";
        //                NumProducts = (int)command.ExecuteScalar();

        //                command.CommandText = "SELECT COUNT(PkVenta_Id) FROM Tbl_Venta WHERE FechaRegistro BETWEEN @fromdate AND @todate";
        //                command.Parameters.Add("@fromdate", System.Data.SqlDbType.DateTime).Value = startDate;
        //                command.Parameters.Add("@todate", System.Data.SqlDbType.DateTime).Value = endDate;
        //                NumOrders = (int)command.ExecuteScalar();
        //            }

        //        }
        //        catch (Exception )
        //        {
        //            lista = new List<Dashboard>();
        //        }

        //        return lista;
        //    }
        //}
    }
}
