using System.Collections.Generic;
using System.Windows.Documents;

namespace Blackbird.WPF.API.Models
{
    public class PinPointResult
    {
        public string District { get; set; }
        public bool IsInTunnel { get; set; }
        public string Tasks { get; set; }

        public List<Task> GetTasks()
        {
            if (string.IsNullOrEmpty(Tasks))
                return null;

            var tasksList= new List<Task>();

            var tasks = Tasks.Split('#');
            foreach (var task in tasks)
            {
                if(string.IsNullOrEmpty(task))
                    continue;

                var split = task.Split(',');
                tasksList.Add(new Task { TaskName = split[0].Trim(), Who = split[1].Trim(), Number = split[2].Trim() });
            }

            return tasksList;
        }
    }
}
