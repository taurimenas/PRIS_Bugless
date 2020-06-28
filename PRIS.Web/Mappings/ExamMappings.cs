using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System.Linq;

namespace PRIS.Web.Mappings
{
    public static class ExamMappings
    {
        public static Exam ToEntity(ExamViewModel model)
        {
            return new Exam
            {
                Comment = model.Comment,
                Date = model.Date,
                Id = model.Id,
                CityId = model.CityId
            };
        }
        public static ExamViewModel ToViewModel(Exam entity)
        {
            return new ExamViewModel
            {
                Comment = entity.Comment,
                Date = entity.Date,
                Id = entity.Id,
                CityId = entity.CityId
            };
        }
    }
}
