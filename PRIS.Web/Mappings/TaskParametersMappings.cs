﻿using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class TaskParametersMappings
    {
        public static Exam ToTaskParametersEntity(SetTaskParameterModel model)
        {
            return new Exam
            {
                Task1_1 = model.Task1_1,
                Task1_2 = model.Task1_2,
                Task1_3 = model.Task1_3,
                Task2_1 = model.Task2_1,
                Task2_2 = model.Task2_2,
                Task2_3 = model.Task2_3,
                Task3_1 = model.Task3_1,
                Task3_2 = model.Task3_2,
                Task3_3 = model.Task3_3,
                Task3_4 = model.Task3_4
            };
        }
        public static TaskParameterViewModel ToTaskParameterViewModel(List<Exam> examEntity)
        {
            return new TaskParameterViewModel
            {
                Tasks = examEntity
            };
        }
    }
}
