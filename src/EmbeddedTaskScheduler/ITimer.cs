using System.Threading;

namespace EmbeddedTaskScheduler
{
    public interface ITimer
    {
        void Start(TimerCallback callback, int period);
    }
}