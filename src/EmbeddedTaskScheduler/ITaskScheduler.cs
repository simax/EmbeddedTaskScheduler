using System.Collections.Generic;

namespace EmbeddedTaskScheduler
{
    public interface ITaskScheduler
    {
        /// <summary>
        ///     How often in milleseconds the task scheduler checks to see if a task should be run.    
        /// </summary>
        int Granularity { get; set; }

        /// <summary>
        ///     How long in milliseconds to wait for a task to complete.
        /// </summary>
        int ProcessTimeout { get; set; }

        /// <summary>
        ///     Gets a list of currently scheduled tasks.
        /// </summary>
        List<Task> TaskList { get; }

        /// <summary>
        ///     Initialises and starts the task scheduler.
        /// </summary>
        /// <param name="configFilePath">Path to XML file that contains the task configuration.</param>
        void Init(string configFilePath);

        /// <summary>
        ///     Initialises and starts the task scheduler.
        /// </summary>
        /// <param name="tasks">List of scheduled tasks</param>
        void Init(List<Task> tasks);
    }
}