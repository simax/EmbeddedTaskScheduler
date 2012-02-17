namespace EmbeddedTaskScheduler
{
    public interface ITaskExecutor
    {
        void Execute(Task task, int timeout);
    }
}