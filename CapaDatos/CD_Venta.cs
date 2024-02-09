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
    public class CD_Venta
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT COUNT(*) + 1 FROM Tbl_Venta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    idcorrelativo = 0;
                }
            }

            return idcorrelativo;
        }

        public bool RestarStock(int idproducto,int cantidad)
        {
            bool respuesta = true;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("UPDATE tbl_Producto SET Stock = Stock - @cantidad WHERE PkProducto_Id = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return respuesta;
        }

        public bool SumarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("UPDATE tbl_Producto SET Stock = Stock + @cantidad WHERE PkProducto_Id = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return respuesta;
        }



        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje, /*borar*/ DataTable idborrar )
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARVENTA", oconexion);
                    cmd.Parameters.AddWithValue("idusuario", obj.oUsuario.PkUsuario_Id);
                    cmd.Parameters.AddWithValue("tipodocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("numerodocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("documentocliente", obj.DocumentoCliente);
                    cmd.Parameters.AddWithValue("nombrecliente", obj.NombreCliente);
                    cmd.Parameters.AddWithValue("montopago", obj.MontoPago);
                    cmd.Parameters.AddWithValue("montocambio", obj.MontoCambio);
                    cmd.Parameters.AddWithValue("montototal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("detalleventa", DetalleVenta);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, (500)).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("idproducto", idborrar);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Mensaje = ex.Message;
                }
                return respuesta;
            }
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta obj = new Venta();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT v.PkVenta_Id,u.Nombre,u.Apellidos,");
                    query.AppendLine("v.DocumentoCliente, v.NombreCliente,");
                    query.AppendLine("v.TipoDocumento,v.NumeroDocumento,");
                    query.AppendLine("v.MontoPago,v.MontoCambio,V.MontoTotal,");
                    query.AppendLine("CONVERT(CHAR(10), V.FechaRegistro, 103)[FechaRegistro]");
                    query.AppendLine("FROM Tbl_Venta v");
                    query.AppendLine("INNER JOIN Tbl_Usuario u on u.PkUsuario_Id = v.FkUsuario_Id");
                    query.AppendLine("WHERE v.NumeroDocumento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Venta()
                            {
                                PkVenta_Id = int.Parse(dr["PkVenta_Id"].ToString()),
                                oUsuario = new Usuario() {Nombre = dr["Nombre"].ToString(), Apellidos = dr["Apellidos"].ToString()},
                                DocumentoCliente = dr["DocumentoCliente"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                MontoPago = Convert.ToDecimal(dr["MontoPago"].ToString()),
                                MontoCambio = Convert.ToDecimal(dr["MontoCambio"].ToString()),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                                FechaRegistro = dr["FechaRegistro"].ToString()

                            };
                        }    
                    }
                }
                catch (Exception ex)
                {
                    obj = new Venta();
                }
            }

            return obj;
        }

        public List<Detalle_Venta> ObtenerDetalleVenta(int idventa)
        {
            List<Detalle_Venta> oLista = new List<Detalle_Venta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.Nombre,dv.PrecioVenta,dv.cantidad,dv.SubTotal");
                    query.AppendLine("FROM Tbl_DetalleVenta dv");
                    query.AppendLine("INNER JOIN tbl_Producto p on p.PkProducto_Id = dv.FkProducto_Id");
                    query.AppendLine("WHERE dv.FkVenta_Id = @idventa");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idventa", idventa);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            oLista.Add(new Detalle_Venta()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                SubTotal = Convert.ToDecimal(dr["SubTotal"].ToString())
                            });
                        }
                    }
                }
                catch
                {
                    oLista = new List<Detalle_Venta>();
                }
            }

                return oLista;
        }
    }
}
