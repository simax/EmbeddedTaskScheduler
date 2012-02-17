using System;

namespace EmbeddedTaskScheduler
{
    public class Task
    {
        /// <summary>
        ///     Path to the executable task to be run
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        ///     Command line arguments to the task to be run
        /// </summary>
        public string Arguments { get; set; }
        /// <summary>
        ///     Description of the task
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///     How often the task should be executed (in seconds)
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        ///     When the task was last executed
        /// </summary>
        public DateTime LastRunTime { get; set; }
        /// <summary>
        ///     When the task should be executed next
        /// </summary>
        public DateTime NextRunTime { get; set; }
        /// <summary>
        ///     How long the task took to execute last time it ran (in milliseconds)
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        ///     The exit code of the executable last time it ran
        /// </summary>
        public int ExitCode { get; set; }
        /// <summary>
        ///     Is the task currently executing
        /// </summary>
        public bool IsRunning { get; set; }
        /// <summary>
        ///     Did the task fail last time it ran
        /// </summary>
        public bool Failed { get; set; }
        /// <summary>
        ///     Did the task timeout last time it was run
        /// </summary>
        public bool TimedOut { get; set; }
    }
}