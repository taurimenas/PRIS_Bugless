using Microsoft.AspNetCore.Mvc.Rendering;
using MoreLinq;
using PRIS.Core.Library.Entities;
using PRIS.Web.Mappings;
using PRIS.Web.Models.InvitationToStudyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Controllers
{
    public class StudentData
    {
        public static StudentInvitationToStudyListViewModel PrepareData(string examId, string courseId, string cityId, string searchString, string sortOrder, ref List<Student> students)
        {
            var stringAcceptancePeriods = DropdownForExams(students);
            var stringCities = DropdownForCities(students);
            DropdownForCourses(students, out List<Course> courses, out List<SelectListItem> stringCourses);

            students = StudentFilterBySelectedExam(examId, students);
            students = StudentFilterBySelectedCourse(courseId, students, courses);
            students = StudentFilterBySelectedCity(cityId, students);

            var invitationToStudy = CreateViewModelList(students);

            invitationToStudy = SearchForStudents(searchString, invitationToStudy);

            var model = StudentInvitationToStudyMappings.ToListViewModel(invitationToStudy);

            SortStudents(sortOrder, invitationToStudy, model);

            AddSelectedAcceptancePeriodToModel(examId, stringAcceptancePeriods, model);
            AddSelectedCityToModel(cityId, stringCities, model);
            AddSelectedCourseToModel(courseId, stringCourses, model);

            return model;
        }

        private static List<StudentInvitationToStudyViewModel> CreateViewModelList(List<Student> students)
        {
            var invitationToStudy = new List<StudentInvitationToStudyViewModel>();
            students.ForEach(x => invitationToStudy
                .Add(StudentInvitationToStudyMappings.StudentInvitationToStudyToViewModel(x, x.ConversationResult, x.StudentCourses, x.Result)));
            return invitationToStudy;
        }

        private static void AddSelectedCourseToModel(string courseId, List<SelectListItem> stringCourses, StudentInvitationToStudyListViewModel model)
        {
            model.Courses = stringCourses;
            var selectedCourse = stringCourses.FirstOrDefault(x => x.Text == courseId);
            model.SelectedPriority = selectedCourse?.Text;
        }

        private static void AddSelectedCityToModel(string cityId, List<SelectListItem> stringCities, StudentInvitationToStudyListViewModel model)
        {
            model.Cities = stringCities;
            var selectedCity = stringCities.FirstOrDefault(x => x.Text == cityId);
            model.SelectedCity = selectedCity?.Text;
        }

        private static void AddSelectedAcceptancePeriodToModel(string examId, List<SelectListItem> stringAcceptancePeriods, StudentInvitationToStudyListViewModel model)
        {
            model.AcceptancePeriods = stringAcceptancePeriods;
            var selectedAcceptancePeriods = stringAcceptancePeriods.FirstOrDefault(x => x.Text == examId);
            model.SelectedAcceptancePeriod = selectedAcceptancePeriods?.Text;
        }

        private static void SortStudents(string sortOrder, List<StudentInvitationToStudyViewModel> invitationToStudy, StudentInvitationToStudyListViewModel model)
        {
            var sort = new Dictionary<string, List<StudentInvitationToStudyViewModel>>
            {
                { "PercentageGrade", invitationToStudy.OrderByDescending(s => s.PercentageGrade).ToList() },
                { "ConversationGrade", invitationToStudy.OrderByDescending(s => s.ConversationGrade).ToList() },
                { "FinalAverageGrade", invitationToStudy.OrderByDescending(s => s.FinalAverageGrade).ToList() },
                { "Priority", invitationToStudy.OrderByDescending(s => s.Priority).ToList() }
            };

            if (sortOrder != null)
            {
                if (sort.ContainsKey(sortOrder))
                    model.StudentInvitationToStudy = sort[sortOrder];
            }
        }

        private static List<StudentInvitationToStudyViewModel> SearchForStudents(string searchString, List<StudentInvitationToStudyViewModel> invitationToStudy)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                if (invitationToStudy.Where(s => s.LastName.Contains(searchString)).Count() == 0)
                    invitationToStudy = invitationToStudy.Where(s => s.FirstName.Contains(searchString)).ToList();
                else invitationToStudy = invitationToStudy.Where(s => s.LastName.Contains(searchString)).ToList();
            }

            return invitationToStudy;
        }

        private static List<Student> StudentFilterBySelectedCity(string cityId, List<Student> students)
        {
            if (cityId != null)
            {
                students = students.Where(e => e.Result.Exam.City.Name == cityId).ToList();
            }

            return students;
        }

        private static List<Student> StudentFilterBySelectedCourse(string courseId, List<Student> students, List<Course> courses)
        {
            if (courseId != null)
            {
                var selected = courses.FirstOrDefault(x => $"{x.Title} {x.StartYear.Year}" == courseId);
                students = students.Where(x => x.StudentCourses.FirstOrDefault(x => x.Priority == 1).Course == selected).ToList();
            }

            return students;
        }

        private static List<Student> StudentFilterBySelectedExam(string examId, List<Student> students)
        {
            if (examId != null)
            {
                students = students.Where(x => x.Result.Exam.AcceptancePeriod == examId).ToList();
            }

            return students;
        }

        private static void DropdownForCourses(List<Student> students, out List<Course> courses, out List<SelectListItem> stringCourses)
        {
            courses = students.Select(x => x.StudentCourses?.FirstOrDefault(y => y.Priority == 1)?.Course).DistinctBy(x => $"{x.Title} {x.StartYear.Year}").ToList();
            stringCourses = new List<SelectListItem>();
            foreach (var p in courses)
            {
                stringCourses?.Add(new SelectListItem { Value = courses?.FindIndex(a => a == p).ToString(), Text = $"{p.Title} {p.StartYear.Year}" });
            }
            stringCourses.Add(new SelectListItem { Value = null, Text = "" });

        }

        private static List<SelectListItem> DropdownForCities(List<Student> students)
        {
            var cities = students.Select(x => x.Result.Exam.City).DistinctBy(x => x.Name).ToList();
            var stringCities = new List<SelectListItem>();
            foreach (var c in cities)
            {
                stringCities.Add(new SelectListItem { Value = cities.FindIndex(a => a == c).ToString(), Text = c.Name });
            }
            stringCities.Add(new SelectListItem { Value = null, Text = "" });

            return stringCities;
        }

        private static List<SelectListItem> DropdownForExams(List<Student> students)
        {
            var stringAcceptancePeriods = new List<SelectListItem>();
            var filteredExams = students.Select(x => x.Result.Exam).DistinctBy(x => x.AcceptancePeriod).ToList();
            foreach (var ed in filteredExams)
            {
                stringAcceptancePeriods.Add(new SelectListItem { Value = filteredExams.FindIndex(a => a == ed).ToString(), Text = ed.AcceptancePeriod });
            }
            stringAcceptancePeriods.Add(new SelectListItem { Value = null, Text = "" });

            return stringAcceptancePeriods;
        }
    }
}
