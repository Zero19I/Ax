using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacion.Dashboard
{
    public struct RevenueByDate
    {
        public string Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ClDashboard : DbConnection
    {
        private DateTime startDate;
        private DateTime endDate;
        private int numberDays;

        public int NumCustomer { get; private set; }
        public int NumSuppliers { get; private set; }
        public int NumProducts { get; private set; }
        public List<KeyValuePair<string,int>> TopProductsList { get; private set; }
        public List<KeyValuePair<string, int>> UnderStockList { get; private set; }
        public List<RevenueByDate> GrossRevenueList { get; private set; }
        public int NumOrders { get; set; }
        public decimal TotalRenueve { get; set; }
        public decimal TotalProfit { get; set; }

        public ClDashboard()
        {
             
        }

        private void GetNumberItems()
        {
            using(var connection = GetConnection())
            {
                connection.Open();

                using(var command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = "SELECT COUNT(PkCliente_Id) FROM Tbl_Cliente";
                    NumCustomer = (int)command.ExecuteScalar();

                    command.CommandText = "SELECT COUNT(PkProveedor_Id) FROM Tbl_Proveedor";
                    NumSuppliers = (int)command.ExecuteScalar();

                    command.CommandText = "SELECT COUNT(PkProducto_Id) FROM tbl_Producto";
                    NumProducts = (int)command.ExecuteScalar();

                    command.CommandText = "SELECT COUNT(PkVenta_Id) FROM Tbl_Venta WHERE FechaRegistro BETWEEN @fromdate AND @todate";
                    command.Parameters.Add("@fromdate", System.Data.SqlDbType.DateTime).Value = startDate;
                    command.Parameters.Add("@todate", System.Data.SqlDbType.DateTime).Value = endDate;
                    NumOrders = (int)command.ExecuteScalar();
                }
            }
        }

        private void GetOrderAnalisys()
        {
            GrossRevenueList = new List<RevenueByDate>();
            TotalProfit = 0;
            TotalRenueve = 0;

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"SELECT FechaRegistro,SUM(MontoTotal) 
                                            FROM Tbl_Venta WHERE FechaRegistro BETWEEN @fromdate AND @todate
                                            GROUP BY FechaRegistro";

                    command.Parameters.Add("@fromdate", System.Data.SqlDbType.DateTime).Value = startDate;
                    command.Parameters.Add("@todate", System.Data.SqlDbType.DateTime).Value = endDate;

                    var reader = command.ExecuteReader();
                    var resultTable = new List<KeyValuePair<DateTime, decimal>>();

                    while(reader.Read())
                    {
                        resultTable.Add(new KeyValuePair<DateTime, decimal>((DateTime)reader[0], (decimal)reader[1])
                            );

                        TotalRenueve += (decimal)reader[1];
                    }

                    TotalProfit = TotalRenueve * 0.2m;/* GANANCIA DEL 20 %*/
                    reader.Close();

                    //AGRUPAR POR DIAS
                    if(numberDays <= 30)
                    {
                        foreach (var item in resultTable)
                        {
                            GrossRevenueList.Add(new RevenueByDate()
                            {
                                Date = item.Key.ToString("dd MMM"),
                                TotalAmount = item.Value
                            });
                        }
                    }

                    //AGRUPAR POR SEMANA
                    else if(numberDays <= 92)
                    {
                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                                orderList.Key, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                           into Order
                                            select new RevenueByDate
                                            {
                                                Date = "Week " + Order.Key.ToString(),
                                                TotalAmount = Order.Sum(amount => amount.Value)
                                            }).ToList();
                    }

                    //AGRUPAR POR MESES
                    else if (numberDays <= (365 * 2))
                    {
                        bool isYear = numberDays <= 365 ? true : false;

                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by orderList.Key.ToString("MMM yyyy")
                                           into Order
                                            select new RevenueByDate
                                            {
                                                Date = isYear ? Order.Key.Substring(0, Order.Key.IndexOf(" ")) : Order.Key,
                                                TotalAmount = Order.Sum(amount => amount.Value)
                                            }).ToList();
                    }

                    //AGRUPAR POR AÑOS
                    else
                    {
                        GrossRevenueList = (from orderList in resultTable
                                            group orderList by orderList.Key.ToString("yyyy")
                                            into Order
                                            select new RevenueByDate
                                            {
                                                Date = Order.Key,
                                                TotalAmount = Order.Sum(amount => amount.Value)
                                            }).ToList();
                    }
                }
            }
        }

        private void GetProductAnalisys()
        {
            TopProductsList = new List<KeyValuePair<string, int>>();
            UnderStockList = new List<KeyValuePair<string, int>>();


            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    SqlDataReader reader;
                    command.Connection = connection;

                    //OBTENER LOS 5 PRODUCTOS MAS VENDIDOS
                    command.CommandText = @"SELECT top 5 p.Nombre, SUM(dv.cantidad) AS Cant FROM  Tbl_DetalleVenta dv
                                            INNER JOIN tbl_Producto p ON p.PkProducto_Id = dv.FkProducto_Id
                                            INNER JOIN Tbl_Venta v ON V.PkVenta_Id = dv.FkVenta_Id
                                            WHERE dv.FechaRegistro BETWEEN @fromdate and @todate
                                            GROUP BY p.Nombre
                                            ORDER BY Cant DESC";
                    command.Parameters.Add("@fromdate", System.Data.SqlDbType.DateTime).Value = startDate;
                    command.Parameters.Add("@todate", System.Data.SqlDbType.DateTime).Value = endDate;
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        TopProductsList.Add(new KeyValuePair<string, int>(reader[0].ToString(), (int)reader[1]));
                    }
                    reader.Close();

                    //OBTIENE LOS PRODUCTOS EN BAJO STOCK O MENOS VENDIDOS O PRODUCTO INACTIVOS QUE EXISTAN IGUAL MENOR A 6
                    command.CommandText = "SELECT Nombre, Stock FROM tbl_Producto WHERE Stock<=6 AND Estado=0";
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UnderStockList.Add(new KeyValuePair<string, int>(reader[0].ToString(), (int)reader[1]));
                    }
                    reader.Close();
                }
            }
        }

        //METODO PUBLICO PARA CARGAR LOS DATOS
        public bool LoadData(DateTime starDate, DateTime endDate)
        {
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, 59);

            if (starDate != this.startDate || endDate != this.endDate)
            {
                this.startDate = starDate;
                this.endDate = endDate;
                this.numberDays = (endDate - starDate).Days;

                GetNumberItems();
                GetOrderAnalisys();
                GetProductAnalisys();

                Console.WriteLine("Refreshed data: {0} - {1}", starDate.ToString(), endDate.ToString());
                return true;
            }
            else
            {
                Console.WriteLine("Data not refreshed, same query: {0} - {1}", starDate.ToString(),endDate.ToString());
                return false;
            }
        }
    }
}
