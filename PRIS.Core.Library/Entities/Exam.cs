using System;
using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class Exam : EntityBase
    {
        public int CityId { get; set; }
        public DateTime Date { get; set; }
        public string Tasks { get; set; }
        public string Comment { get; set; }
        public string AcceptancePeriod { get; set; }

        public List<Result> Results { get; set; }
        public City City { get; set; }
    }
}
