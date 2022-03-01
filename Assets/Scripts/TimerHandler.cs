using System.Collections;
using UnityEngine;

class TimerHandler
{
    public float Countdown { get; set; }

    public delegate void TimerDelegate();

    public event TimerDelegate Ended;

    public IEnumerator Timer(float time)
    {
        Countdown = time;
        while (Countdown > 0)
        {
            Countdown -= Time.deltaTime;
            yield return 0;
        }

        if (Ended != null) Ended.Invoke();
    }
}
