using System;

namespace EmbeddedTaskScheduler
{
    public class Task
    {
        public string Path { get; set; }
        public string Arguments { get; set; }
        public string Description { get; set; }
        public int Frequency { get; set; }
        public DateTime LastRunTime { get; set; }
        public DateTime NextRunTime { get; set; }
        public double Duration { get; set; }
        public int ExitCode { get; set; }
        public bool IsRunning { get; set; }
        public bool Failed { get; set; }
        public bool TimedOut { get; set; }
    }
}