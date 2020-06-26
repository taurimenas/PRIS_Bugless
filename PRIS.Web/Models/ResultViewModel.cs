using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ResultViewModel
    {

        public int Id { get; set; }
        public double Task1_1 { get; set; }
        public double Task1_2 { get; set; }
        public double Task1_3 { get; set; }
        public double Task2_1 { get; set; }
        public double Task2_2 { get; set; }
        public double Task2_3 { get; set; }
        public double Task3_1 { get; set; }
        public double Task3_2 { get; set; }
        public double Task3_3 { get; set; }
        public double Task3_4 { get; set; }
        public double FinalPoints { get; set; }
        public string Comment { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


    }
}
