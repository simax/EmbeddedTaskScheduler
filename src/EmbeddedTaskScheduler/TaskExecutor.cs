using System;
using System.Diagnostics;

namespace EmbeddedTaskScheduler
{
    public class TaskExecutor : ITaskExecutor
    {
        public void Execute(Task task, int timeout)
        {
            try
            {
                var process = Process.Start(task.Path, task.Arguments);
                process.WaitForExit(timeout);

                if (!process.HasExited)
                {
                    process.Kill();
                    task.TimedOut = true;
                }

                task.Duration = (process.ExitTime - process.StartTime).TotalMilliseconds;
                task.ExitCode = process.ExitCode;
            }
            catch (Exception)
            {
                task.Failed = true;
            }
        }
    }
}