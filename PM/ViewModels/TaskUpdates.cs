
using System.Collections.Generic;

using PM.Models;

namespace PM.ViewModels
{
    public class TaskUpdates
    {
        public int? task_id { get; set; }
        public int? sub_task_id { get; set; }
        public List<int> tags { get; set; }
        public List<int> assignees { get; set; }
        public List<int> assignees_new { get; set; }
        public int? status { get; set; }
    }
}