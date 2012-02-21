using System;
using System.Diagnostics;
using System.IO;

namespace EmbeddedTaskScheduler
{
    public class TaskExecutor : ITaskExecutor
    {
        public void Execute(Task task, int timeout)
        {
            try
            {
                if (!File.Exists(task.Path))
                {
                    task.Failed = true;
                    return;
                }

                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = task.Path,
                    Arguments = task.Arguments,
                    WorkingDirectory = Path.GetDirectoryName(task.Path)
                });

                process.WaitForExit(timeout);

                if (!process.HasExited)
                {
                    process.Kill();
                    task.TimedOut = true;
                    task.Failed = true;
                }

                task.Duration = Convert.ToInt32((process.ExitTime - process.StartTime).TotalMilliseconds);
                task.ExitCode = process.ExitCode;

                task.Failed = false;
                task.TimedOut = false;
            }
            catch (Exception)
            {
                task.Failed = true;
            }
        }
    }
}