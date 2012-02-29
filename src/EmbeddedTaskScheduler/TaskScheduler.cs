using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EmbeddedTaskScheduler
{
    public class TaskScheduler : ITaskScheduler
    {
        ITimer _timer;
        ITaskRunner _taskRunner;

        /// <summary>
        ///     How often in milleseconds the task scheduler checks to see if a task should be run.    
        /// </summary>
        public int Granularity { get; set; }

        /// <summary>
        ///     How long in milliseconds to wait for a task to complete.
        /// </summary>
        public int ProcessTimeout { get; set; }

        /// <summary>
        ///     Gets a list of currently scheduled tasks.
        /// </summary>
        public List<Task> TaskList { get; private set; }

        public TaskScheduler() : this(new ThreadedTimer(), new TaskRunner(new TaskExecutor()))
        {
            
        }

        public TaskScheduler(ITimer timer, ITaskRunner taskRunner)
        {
            _timer = timer;
            _taskRunner = taskRunner;

            ProcessTimeout = 1000 * 60 * 10;
            Granularity = 1000 * 5;
        }

        /// <summary>
        ///     Initialises and starts the task scheduler.
        /// </summary>
        /// <param name="configFilePath">Path to XML file that contains the task configuration.</param>
        public void Init(string configFilePath)
        {
            Init(ParseConfig(LoadConfig(configFilePath)));
        }
        
        /// <summary>
        ///     Initialises and starts the task scheduler.
        /// </summary>
        /// <param name="tasks">List of scheduled tasks</param>
        public void Init(List<Task> tasks)
        {
            TaskList = InitialiseTasks(tasks);
            _timer.Start(TimerCallback, Granularity);
        }

        private string LoadConfig(string configFilePath)
        {
            return File.ReadAllText(configFilePath);
        }

        private List<Task> ParseConfig(string configData)
        {
            return XDocument.Parse(configData).Root.Elements().Select(t => new Task
            {
                Path = t.Element("Path").Value,
                Arguments = t.Element("Arguments").Value,
                Description = t.Element("Description").Value,
                Frequency = Convert.ToInt32(t.Element("Frequency").Value)

            }).ToList();
        }

        private List<Task> InitialiseTasks(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                task.LastRunTime = DateTime.Now.AddSeconds(task.Frequency);
                task.NextRunTime = DateTime.Now.AddSeconds(task.Frequency);
            }

            return tasks;
        }

        private void TimerCallback(object state)
        {
            _taskRunner.RunNextTask(TaskList, ProcessTimeout);
        }
    }
}