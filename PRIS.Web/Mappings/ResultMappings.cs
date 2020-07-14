using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class ResultMappings
    {
        public static ResultViewModel ToViewModel(Result result)
        {
            var finalPoints = JsonSerializer.Deserialize<double[]>(result.Tasks).Sum(x => x);
            double maxPoints = JsonSerializer.Deserialize<double[]>(result.Exam.Tasks).Sum(x => x);
            var percentageGrade = finalPoints * 100 / maxPoints;
            return new ResultViewModel
            {
                ResultId = result.Id,
                CommentResult = result.Comment,
                ExamId = result.ExamId,
                StudentForeignKey = result.StudentForeignKey,
                Tasks = JsonSerializer.Deserialize<double[]>(result.Tasks),
                FinalPoints = finalPoints,
                PercentageGrade = percentageGrade
            };
        }
    }
}
