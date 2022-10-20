using System;
using TMPro;
using UnityEngine;

public interface ITimer
{
    void StartTimer(Action callback);
    void StopTimer();
    float Time { get; }
}

public class TimerDisplay : MonoBehaviour, ITimer
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private float duration;
    private bool _running;
    private float _time;
    private float _prevTime;

    private Action _callback;

    public void StartTimer(Action callback)
    {
        _callback = callback;
        _running = true;
        _time = 0;
        _prevTime = 0;
        DisplayTime(_time);
    }

    public void StopTimer()
    {
        _running = false;
    }

    public float Time => Mathf.RoundToInt(_time);

    private void Update()
    {
        if (_running)
        {
            _time += UnityEngine.Time.deltaTime;
            if (_time - _prevTime >= 1f)
            {
                _prevTime = _time;
                DisplayTime(_time);
            }
        }

        if (_time >= duration)
        {
            StopTimer();
            DisplayTime(duration);
            _callback?.Invoke();
        }
    }

    public void DisplayTime(float time)
    {
        text.text = $"{Mathf.RoundToInt(duration - time)}s";
    }
}