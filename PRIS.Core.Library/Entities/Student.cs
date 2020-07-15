using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class Student : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Comment { get; set; }
        public bool PassedExam { get; set; }
        public bool SignedAContract { get; set; }
        public bool InvitedToStudy { get; set; }
        public bool StudentDataLocked { get; set; }
        public int? StudentsCourseId { get; set; }
        public int? ResultId { get; set; }
        public int? ConversationResultId { get; set; }

        public Result Result { get; set; }
        public ConversationResult ConversationResult { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
    }
}
