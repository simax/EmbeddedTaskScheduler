using System;
using System.Collections.Generic;
using System.Linq;

namespace EmbeddedTaskScheduler
{
    public class TaskRunner : ITaskRunner
    {
        ITaskExecutor _taskExecutor;
        bool _taskRunning;

        public TaskRunner(ITaskExecutor taskExecutor)
        {
            _taskExecutor = taskExecutor;
        }

        public void RunNextTask(IEnumerable<Task> tasks, int timeout)
        {
            if (_taskRunning) return;

            var task = getHighestPriorityTask(tasks);

            if (task.NextRunTime > DateTime.Now) return;

            _taskRunning = true;

            task.IsRunning = true;
            task.LastRunTime = DateTime.Now;

            _taskExecutor.Execute(task, timeout);

            task.IsRunning = false;
            task.NextRunTime = DateTime.Now.AddSeconds(task.Frequency);

            _taskRunning = false;
        }

        private Task getHighestPriorityTask(IEnumerable<Task> taskList)
        {
            return taskList.OrderBy(t => t.NextRunTime).First();
        }
    }
}