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
    public class CD_Permiso
    {
        public List<Permiso> Listar( int idusuario)
        {
            List<Permiso> Lista = new List<Permiso>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select p.FkRol_Id,p.NombreMenu from Tbl_Permiso p");
                    query.AppendLine("inner join Tbl_Rol r on r.PkRol_Id = p.FkRol_Id");
                    query.AppendLine("inner join Tbl_Usuario u on  u.FkRol_Id = r.PkRol_Id");
                    query.AppendLine("where u.PkUsuario_Id = @idusuario");



                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Permiso()
                            {
                                FkRol_Id = new Rol() { PkRol_Id = Convert.ToInt32(dr["FkRol_Id" /*revisar*/ ]) },
                                NombreMenu = dr["NombreMenu"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lista = new List<Permiso>();
                }
            }
            return Lista;
        }
    }
}
