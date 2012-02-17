using System.Collections.Generic;

namespace EmbeddedTaskScheduler
{
    public interface ITaskRunner
    {
        void RunNextTask(IEnumerable<Task> tasks, int timeout);
    }
}