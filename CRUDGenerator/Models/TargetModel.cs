using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDGenerator.Models
{
    public class License
    {
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
    public class DependencyListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
