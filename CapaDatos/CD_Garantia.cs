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
    public class CD_Garantia
    {
        public List<GarantiaCliente> Listar()
        {
            List<GarantiaCliente> ListaCliente = new List<GarantiaCliente>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine(@"SELECT dv.PkDetalleVenta_Id,p.PkProducto_Id,v.NumeroDocumento, p.Nombre,dv.cantidad, 
                                    CONVERT(CHAR(10),g.FechaCompra,103)[FechaCompra], CONVERT(CHAR(10), g.FechaVencimiento,103)[FechaVencimiento], g.Estado 
                                    FROM Tbl_Venta v
                                    INNER JOIN Tbl_DetalleVenta dv ON dv.FkVenta_Id = v.PkVenta_Id
                                    INNER JOIN tbl_Producto p ON p.PkProducto_Id = dv.FkProducto_Id
                                    INNER JOIN tbl_Categoria c ON c.PkCategoria_Id = p.FkCategoria_Id
                                    INNER JOIN Tbl_Marca m ON m.PkMarca_Id = p.FkMarca_Id
                                    INNER JOIN Tbl_Garantia g ON g.IdVenta = v.PkVenta_Id AND G.IdProducto = P.PkProducto_Id
                                    WHERE p.FkCategoria_Id = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ListaCliente.Add(new GarantiaCliente()
                            {
                                PkDetalleVenta_Id = Convert.ToInt32(dr["PkDetalleVenta_Id"]),
                                PkProducto_Id = Convert.ToInt32(dr["PkProducto_Id"]),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                Producto = dr["Nombre"].ToString(),
                                Cantidad = Convert.ToInt32(dr["cantidad"]),
                                FechaInicio = dr["FechaCompra"].ToString(),
                                FechaLimite = dr["FechaVencimiento"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ListaCliente = new List<GarantiaCliente>();
                }
            }
            return ListaCliente;
        }
        public bool RegistrarProductoRIP(GarantiaCliente obj)
        {
            bool respuesta = false;

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARPRODUCTODEFECTUOSO", oConexion);
                    cmd.Parameters.AddWithValue("IdDetalleVenta", obj.PkDetalleVenta_Id);
                    cmd.Parameters.AddWithValue("IdProducto", obj.PkProducto_Id);
                    cmd.Parameters.AddWithValue("Cantidad", obj.Cantidad);
                    cmd.Parameters.AddWithValue("IdVenta", obj.NumeroDocumento);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public List<GarantiaProveedor> ListarGP()
        {
            List<GarantiaProveedor> ListarGP = new List<GarantiaProveedor>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine(@"SELECT pr.RazonSocial, dc.PrecioCompra, p.Nombre, pm.Cantidad, p.PkProducto_Id
                                        FROM Tbl_Proveedor pr 
                                        INNER JOIN Tbl_Compra c ON c.FkProveedor_Id = pr.PkProveedor_Id
                                        INNER JOIN Tbl_DetalleCompra dc ON dc.FkCompra_Id = c.PkCompra_Id
                                        INNER JOIN tbl_Producto P ON p.PkProducto_Id = dc.FkProducto_Id
                                        INNER JOIN Tbl_ProductoDefectuoso pm ON pm.FkProducto_Id = p.PkProducto_Id");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ListarGP.Add(new GarantiaProveedor()
                            {
                                Proveedor = dr["RazonSocial"].ToString(),
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"].ToString()),
                                Producto = dr["Nombre"].ToString(),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                idproducto = Convert.ToInt32(dr["PkProducto_Id"].ToString())

                            });
                        }
                    }
                }
                catch (Exception)
                {

                    ListarGP = new List<GarantiaProveedor>();
                }
            }

            return ListarGP;
        }
        public bool RealizarCambioProveedor(GarantiaProveedor obj)
        {
            bool respuesta = false;

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_GARANTIAPROVEEDOR", oConexion);
                    cmd.Parameters.AddWithValue("idproducto", obj.idproducto);
                    cmd.Parameters.AddWithValue("Cantidad", obj.Cantidad);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
