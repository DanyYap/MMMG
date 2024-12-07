using System;
using UnityEngine;

[Serializable]
public class CountdownTimerSettings
{
    [Tooltip("Initial countdown time in seconds.")]
    public float startTimeInSeconds = 60f;

    [Tooltip("Rate at which the countdown decreases (units per second).")]
    public float decreaseRatePerSecond = 1f;
}

public class CountdownTimer
{
    public event Action<float> OnTimeUpdated; // Event triggered when time updates
    public event Action OnCountdownFinished; // Event triggered when countdown finishes

    private float remainingTime;
    private float countdownSpeed;
    private bool isActive;

    public CountdownTimer(CountdownTimerSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));

        remainingTime = settings.startTimeInSeconds;
        countdownSpeed = settings.decreaseRatePerSecond;
        isActive = false;
    }

    public void Start()
    {
        isActive = true;
    }

    public void Pause()
    {
        isActive = false;
    }

    public void Update()
    {
        if (!isActive || remainingTime <= 0) return;

        remainingTime -= countdownSpeed * Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0);

        OnTimeUpdated?.Invoke(remainingTime);

        if (remainingTime <= 0)
        {
            isActive = false;
            OnCountdownFinished?.Invoke();
        }
    }

    public void Reset(float newStartTime)
    {
        remainingTime = newStartTime;
        isActive = false;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }
}

