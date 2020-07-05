using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class SetTaskParameterModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime Date { get; set; }
        public int[] Tasks { get; set; }
        //[Display(Name = "1.1")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task1_1 { get; set; }
        //[Display(Name = "1.2")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task1_2 { get; set; }
        //[Display(Name = "1.3")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task1_3 { get; set; }
        //[Display(Name = "2.1")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task2_1 { get; set; }
        //[Display(Name = "2.2")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task2_2 { get; set; }
        //[Display(Name = "2.3")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task2_3 { get; set; }
        //[Display(Name = "3.1")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task3_1 { get; set; }
        //[Display(Name = "3.2")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task3_2 { get; set; }
        //[Display(Name = "3.3")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task3_3 { get; set; }
        //[Display(Name = "3.4")]
        //[Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        //[Range(double.MinValue, 10, ErrorMessage = "Įveskite {0} užduoties balą iki 10")]
        //public double Task3_4 { get; set; }
    }
}
