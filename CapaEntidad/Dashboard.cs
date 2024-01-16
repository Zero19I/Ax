using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public struct RevenueByDate
    {
        public string Date { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class Dashboard
    {
        private DateTime startDate;
        private DateTime endDate;
        private int numberDays;

        public int NumCustomer { get; set; }
        public int NumSuppliers { get; set; }
        public int NumProducts { get; set; }
        public List<KeyValuePair<string, int>> TopProductsList { get; private set; }
        public List<KeyValuePair<string, int>> UnderStockList { get; private set; }
        public List<RevenueByDate> GrossRevenueList { get; private set; }
        public int NumOrders { get; set; }
        public decimal TotalRenueve { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
