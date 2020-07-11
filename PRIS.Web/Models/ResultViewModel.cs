using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ResultViewModel
    {
        public int ResultId { get; set; }
        public double[] Tasks { get; set; }
        [Display(Name = "Testo balai")]
        public double? FinalPoints { get; set; }
        [Display(Name = "Procentai")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? PercentageGrade { get; set; }
        [Display(Name = "Komentaras")]
        public string CommentResult { get; set; }
        public int? StudentForeignKey { get; set; }
        public int? ExamId { get; set; }
    }
}
