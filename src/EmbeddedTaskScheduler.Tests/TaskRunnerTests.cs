using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace EmbeddedTaskScheduler.Tests
{
    [TestFixture]
    public class TaskRunnerTests
    {
        List<Task> _tasks;
        Mock<ITaskExecutor> _taskExecutor;
        TaskRunner _taskRunner;

        [SetUp]
        public void SetUp()
        {
            _tasks = CreateTasks();

            _taskExecutor = new Mock<ITaskExecutor>();

            _taskRunner = new TaskRunner(_taskExecutor.Object);
        }

        [Test]
        public void Next_Highest_Priority_Task_Is_Executed_First()
        {
            var nextTask = _tasks.OrderBy(t => t.Frequency).First();
            var originalNextRunTime = nextTask.NextRunTime;

            _taskRunner.RunNextTask(_tasks, 0);

            _taskExecutor.Verify(m => m.Execute(nextTask, It.IsAny<int>()));
            Assert.Greater(nextTask.NextRunTime, originalNextRunTime);
        }

        [Test]
        public void No_Tasks_Executed_If_All_Due_To_Be_Run_In_The_Future()
        {
            foreach (var task in _tasks)
            {
                task.NextRunTime = DateTime.MaxValue;
            }

            _taskRunner.RunNextTask(_tasks, 0);

            _taskExecutor.Verify(m => m.Execute(It.IsAny<Task>(), It.IsAny<int>()), Times.Never());
        }

        public List<Task> CreateTasks()
        {
            var tasks = new List<Task>();
            
            for (var i = 0; i < 10; i++)
            {
                tasks.Add(new Task
                {
                    Description = "Task " + i,
                    LastRunTime = DateTime.MinValue,
                    NextRunTime = DateTime.MinValue,
                    Frequency = i * 1000
                });
            }

            return tasks;
        }
    }
}