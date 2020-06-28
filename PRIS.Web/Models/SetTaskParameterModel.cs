using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class SetTaskParameterModel
    {
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task1_1 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task1_2 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task1_3 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task2_1 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task2_2 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task2_3 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task3_1 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task3_2 { get; set; } = 1;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task3_3 { get; set; } = 2;
        [Range(0, double.MaxValue, ErrorMessage = "Balas negali būti mažiau nei 0")]
        [Required]
        public double Task3_4 { get; set; } = 4;
    }
}
