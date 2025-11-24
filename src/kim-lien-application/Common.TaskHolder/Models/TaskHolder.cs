using Common.TaskHolder.Abstractions;

namespace Common.TaskHolder.Models
{
    public class TaskHolder : ITaskHolder
    {
        public List<Task> Tasks { get; set; } = new List<Task>();

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public void AddTask(List<Task> tasks)
        {
            Tasks.AddRange(tasks);
        }

        public void WaitAll()
        {
            Task.WaitAll(Tasks.ToArray());
        }
    }
}
