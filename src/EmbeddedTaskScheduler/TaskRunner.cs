using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EmbeddedTaskScheduler
{
    public class TaskRunner : ITaskRunner
    {
        ITaskExecutor _taskExecutor;
        Mutex _mutex = new Mutex();

        public TaskRunner(ITaskExecutor taskExecutor)
        {
            _taskExecutor = taskExecutor;
        }

        public void RunNextTask(IEnumerable<Task> tasks, int timeout)
        {
            if (!_mutex.WaitOne(0)) return;

            var task = getHighestPriorityTask(tasks);

            if (task.NextRunTime > DateTime.Now) return;

            task.IsRunning = true;
            task.LastRunTime = DateTime.Now;

            _taskExecutor.Execute(task, timeout);

            task.IsRunning = false;
            task.NextRunTime = DateTime.Now.AddSeconds(task.Frequency);

            _mutex.ReleaseMutex();
        }

        private Task getHighestPriorityTask(IEnumerable<Task> taskList)
        {
            return taskList.OrderBy(t => t.NextRunTime).First();
        }
    }
}