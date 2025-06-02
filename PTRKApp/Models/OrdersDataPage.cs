using PTRKApp.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTRKApp.Models
{
    public class OrdersDataPage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Customer { get; set; }

        public string Deadline { get; set; }

        public string StartWorkDate { get; set; }

        public string Services { get; set; }

        public string Group { get; set; }
    }
}
