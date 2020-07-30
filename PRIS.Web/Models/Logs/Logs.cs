using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.Logs
{
    public class Logs
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string TimeStamp { get; set; }
        public string Exception { get; set; }
    }
}
