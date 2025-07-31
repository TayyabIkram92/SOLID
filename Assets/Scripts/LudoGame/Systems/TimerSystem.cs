using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LudoGame.Systems
{
    /// <summary>
    /// Manages timers for player turns and actions
    /// </summary>
    public class TimerSystem : MonoBehaviour, ITimerSystem
    {
        [SerializeField] private Slider timerSlider;
        [SerializeField] private Text timerText;

        private Coroutine _activeTimer;
        private float _currentDuration;
        private Action _onTimeoutCallback;
        private bool _isRunning = false;

        public void StartTimer(float duration, Action onTimeout)
        {
            StopTimer();

            _currentDuration = duration;
            _onTimeoutCallback = onTimeout;
            _isRunning = true;

            // Initialize UI
            if (timerSlider != null)
            {
                timerSlider.gameObject.SetActive(true);
                timerSlider.maxValue = duration;
                timerSlider.value = duration;
            }

            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
                timerText.text = duration.ToString("0");
            }

            _activeTimer = StartCoroutine(RunTimer());
        }

        public void StopTimer()
        {
            if (_activeTimer != null)
            {
                StopCoroutine(_activeTimer);
                _activeTimer = null;
            }

            _isRunning = false;

            // Hide UI
            if (timerSlider != null)
                timerSlider.gameObject.SetActive(false);

            if (timerText != null)
                timerText.gameObject.SetActive(false);
        }

        public bool IsTimerRunning()
        {
            return _isRunning;
        }

        private IEnumerator RunTimer()
        {
            float timeRemaining = _currentDuration;

            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                // Update UI
                if (timerSlider != null)
                    timerSlider.value = timeRemaining;

                if (timerText != null)
                    timerText.text = Mathf.CeilToInt(timeRemaining).ToString();

                yield return null;
            }

            // Timer completed
            _isRunning = false;

            // Hide UI
            if (timerSlider != null)
                timerSlider.gameObject.SetActive(false);

            if (timerText != null)
                timerText.gameObject.SetActive(false);

            // Call timeout callback
            if (_onTimeoutCallback != null)
                _onTimeoutCallback();
        }
    }
}
