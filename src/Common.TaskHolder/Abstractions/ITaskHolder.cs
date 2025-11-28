namespace Common.TaskHolder.Abstractions
{
    public interface ITaskHolder
    {
        void AddTask(Task task);
        void AddTask(List<Task> tasks);
        void WaitAll();
    }
}
