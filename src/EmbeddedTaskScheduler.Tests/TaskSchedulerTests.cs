using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using NUnit.Framework;

namespace EmbeddedTaskScheduler.Tests
{
    [TestFixture]
    public class TaskSchedulerTests
    {
        List<Task> _tasks;
        Mock<ITimer> _timer;
        Mock<ITaskRunner> _taskRunner;
        TaskScheduler _taskScheduler;

        [SetUp]
        public void SetUp()
        {
            _tasks = CreateTasks();

            _timer = new Mock<ITimer>();
            _taskRunner = new Mock<ITaskRunner>();

            _taskScheduler = new TaskScheduler(_timer.Object, _taskRunner.Object);
        }

        [Test]
        public void Task_List_Is_Available_After_Init()
        {
            _taskScheduler.Init(_tasks);
            Assert.AreEqual(_taskScheduler.TaskList, _tasks);
        }

        [Test]
        public void After_Init_All_Tasks_Have_Correct_Next_And_Last_Run_Times()
        {
            _taskScheduler.Init(_tasks);
            
            foreach (var task in _taskScheduler.TaskList)
            {
                Assert.GreaterOrEqual(task.LastRunTime, DateTime.MinValue);
                Assert.GreaterOrEqual(task.NextRunTime, DateTime.MinValue);
            }
        }

        [Test]
        public void After_Init_Timer_Is_Started()
        {
            _taskScheduler.Init(_tasks);
            _timer.Verify(m => m.Start(It.IsAny<TimerCallback>(), It.IsAny<int>()), Times.Once());
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