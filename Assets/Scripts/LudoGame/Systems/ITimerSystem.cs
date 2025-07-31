using System;

namespace LudoGame.Systems
{
    /// <summary>
    /// Interface for timer functionality
    /// </summary>
    public interface ITimerSystem
    {
        void StartTimer(float duration, Action onTimeout);
        void StopTimer();
        bool IsTimerRunning();
    }
}
