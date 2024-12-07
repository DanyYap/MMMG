using System;

public class TimeManager
{
    private CountdownTimer timer;
    
    public TimeManager(GameLevelSettings levelSettings)
    {
        this.timer = new CountdownTimer(levelSettings.countdownTimerSettings);
    }

    public void StartGameTime()
    {
        timer.Start();
    }

    public void UpdateGameTime()
    {
        timer.Update();
    }

    public void StopGameTime()
    {
        timer.Pause();
    }

    public CountdownTimer GetCountdownTimer() => timer;
}
