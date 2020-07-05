using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class TaskParametersMappings
    {
        public static Exam ToTaskParametersEntity(SetTaskParameterModel model)
        {
            return new Exam
            {
                Date = model.Date,
                CityId = model.CityId,
                Tasks = JsonSerializer.Serialize(model),
            };
        }
        public static void EditTaskParametersEntity(Exam exam, int[] tasks)
        {
            exam.Tasks = JsonSerializer.Serialize(tasks);
        }

        public static SetTaskParameterModel ToTaskParameterViewModel(Exam examEntity)
        {
            return new SetTaskParameterModel
            {
                Id = examEntity.Id,
                CityId = examEntity.CityId,
                Date = examEntity.Date,
                Tasks = JsonSerializer.Deserialize<int[]>(examEntity.Tasks)
            };
        }
        public static TaskParameterModel ToTaskViewModel(Exam examEntity)
        {
            return new TaskParameterModel
            {
                Tasks = JsonSerializer.Deserialize<int[]>(examEntity.Tasks)
            };
        }
        public static SetTaskParameterModel ToTaskParameterViewModel(int[] tasks)
        {
            return new SetTaskParameterModel
            {
                Tasks = tasks
            };
        }
    }
}
