using System.Threading;

namespace EmbeddedTaskScheduler
{
    public class ThreadedTimer : ITimer
    {
        Timer _timer;

        public void Start(TimerCallback callback, int period)
        {
            _timer = new Timer(callback, null, 0, period);
        }
    }
}