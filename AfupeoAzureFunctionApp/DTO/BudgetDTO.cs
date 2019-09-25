using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class BudgetDTO
    {
        

        public string id { get; set; }
        public string id_sous_classification { get; set; }
        public string sous_classification { get; set; }
        public string budget { get; set; }
        public DateTime periode { get; set; }
    }
}
