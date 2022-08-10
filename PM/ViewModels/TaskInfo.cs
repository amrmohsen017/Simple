
using System.Collections.Generic;

using PM.Models;

namespace PM.ViewModels
{
    public class TaskInfo
    {
        public task task{ get; set; }
        public List<tag> tags { get; set; }
        public List<attachemnt> attatchments { get; set; }

        public List<task_logs> logs{ get; set; }
        public List<status> state { get; set; }
        public   List<task_assignedemployee> assignees{ get; set; }
    }
}