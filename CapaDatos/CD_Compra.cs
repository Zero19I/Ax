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
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT COUNT(*) + 1 FROM Tbl_Compra");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch(Exception ex)
                {
                    idcorrelativo = 0;
                }
            }

            return idcorrelativo;
        }

        public bool Registrar(Compra obj,DataTable DetalleCompra,out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARCOMPRA", oconexion);
                    cmd.Parameters.AddWithValue("idusuario", obj.oUsuario.PkUsuario_Id);
                    cmd.Parameters.AddWithValue("idproveedor", obj.oProveedor.PkProveedor_Id);
                    cmd.Parameters.AddWithValue("tipodocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("numerodocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("montototal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("detallecompra", DetalleCompra);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, (500)).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

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

        public Compra ObtenerCompra(string numero)
        {
            Compra obj = new Compra();

            using(SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    oconexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT c.PkCompra_Id,u.Nombre,u.Apellidos,p.Documento,p.RazonSocial,c.TipoDocumento,");
                    query.AppendLine("c.NumeroDocumento,c.MontoTotal,CONVERT(CHAR(10),c.FechaRegistro,103)[FechaRegistro] FROM Tbl_Compra c");
                    query.AppendLine("INNER JOIN Tbl_Usuario u on u.PkUsuario_Id = c.FkUsuario_Id");
                    query.AppendLine("INNER JOIN Tbl_Proveedor p on p.PkProveedor_Id = c.FkProveedor_Id");
                    query.AppendLine("WHERE C.NumeroDocumento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            obj = new Compra()
                            {
                                PkCompra_Id = int.Parse(dr["PkCompra_Id"].ToString()),
                                oUsuario = new Usuario() { Nombre = dr["Nombre"].ToString(), Apellidos = dr["Apellidos"].ToString() },
                                oProveedor = new Proveedor() { Documento = dr["Documento"].ToString(), RazonSocial = dr["RazonSocial"].ToString() },
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                                FechaRegistro = dr["FechaRegistro"].ToString()
                            };
                        }
                    }
                    
                }
                catch(Exception ex)
                {
                    obj = new Compra();
                }
            }

            return obj;
        }

        public List<Detalle_Compra> ObtenerDetalleCompra(int idcompra)
        {
            List<Detalle_Compra> oLista = new List<Detalle_Compra>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("SELECT p.Nombre,dc.PrecioCompra,dc.Cantidad,dc.MontoTotal FROM Tbl_DetalleCompra dc");
                    query.AppendLine("INNER JOIN tbl_Producto p on p.PkProducto_Id = dc.FkProducto_Id");
                    query.AppendLine("WHERE dc.FkCompra_Id = @idcompra");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idcompra", idcompra);
                    cmd.CommandType = System.Data.CommandType.Text;
                    

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Compra()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString())
                            });
                        }
                    }
                }

                catch
                {
                    oLista = new List<Detalle_Compra>();
                }
                return oLista;
            }
        }
    }
}
