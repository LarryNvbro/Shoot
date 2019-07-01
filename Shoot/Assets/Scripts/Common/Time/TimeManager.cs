using UnityEngine;
using System;
using Candlelight;
using CodeStage.AntiCheat.ObscuredTypes;

public class TimeManager : Singleton<TimeManager>
{
    private ObscuredLong serverTime; // 서버 시간 값
    private ObscuredLong lastLocalTime; // 로컬(디바이스) 시간

#if UNITY_EDITOR
    [SerializeField]
    private int add_hour;
    [SerializeField]
    private int add_minutes;
    [SerializeField]
    private int add_second;
#endif



    //public DateTime Now { get { return serverTime.AddSeconds(Time.realtimeSinceStartup - prev_time); } }
    public DateTime Now
    {
        get
        {
            DateTime _now = new DateTime(serverTime + (DateTime.Now.Ticks - lastLocalTime));
#if UNITY_EDITOR
            if (add_hour != 0) _now = _now.AddHours(add_hour);
            if (add_minutes != 0) _now = _now.AddMinutes(add_minutes);
            if (add_second != 0) _now = _now.AddSeconds(add_second);
#endif
            return _now;
        }
    }


    [SerializeField]
    private bool pause;
    public bool Pause { get { return pause; } }
    public bool Resume { get { return !pause; } }

    [SerializeField, PropertyBackingField("TimeScale")]
    private float timeScale;
    [Beebyte.Obfuscator.SkipRename]
    public float TimeScale
    {
        get
        {
            return timeScale;
        }
        set
        {
            timeScale = Mathf.Clamp(value, 0.0f, 100.0f);
            Time.timeScale = timeScale;
        }
    }

    public float UnscaledDeletaTime
    {
        get { return pause ? 0.0f : Time.unscaledDeltaTime; }
    }

    [SerializeField]
    private bool isAnimation;

    //public Tweener slowTween;

    public override void Init()
    {
        pause = false;
        timeScale = 1.0f;

        // 기본 현재 로컬타임
        SetServerTimeNow(DateTime.Now);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            TimeScale += 0.1f;
        }

        if (Input.GetKeyUp(KeyCode.KeypadMinus))
        {
            TimeScale -= 0.1f;
        }
    }
#endif

    public void TimePause()
    {
        pause = true;
        Time.timeScale = 0.0f;
    }

    public void TimeResume()
    {
        pause = false;
        Time.timeScale = TimeScale;
    }

    public void SetTimeScale(float scale)
    {
        timeScale = Mathf.Clamp(scale, 0.0f, 100.0f);
        Time.timeScale = pause ? 0.0f : scale;
    }

    public void StartTimeScaleAnimation(float duration = 1.6f)
    {
        //if (!isAnimation)
        //{
        //    isAnimation = true;
        //    TimeScale = 0.1f;
        //    slowTween = HOTween.To(this, duration, new TweenParms().Delay(0.6f).Prop("TimeScale", 1.0f).Ease(EaseType.EaseInCirc).UpdateType(UpdateType.TimeScaleIndependentUpdate).OnComplete(delegate () {
        //        isAnimation = false;
        //        slowTween = null;
        //    }));
        //}
    }

    public void StartTimeScaleTimingShot()
    {
        //if (!isAnimation)
        //{
        //    isAnimation = true;
        //    TimeScale = 0.0f;
        //    slowTween = HOTween.To(this, GameSettings.Get().timingShotSettings.stopDuration, new TweenParms().Delay(GameSettings.Get().timingShotSettings.stopDelay).Prop("TimeScale", 1.0f).Ease(EaseType.EaseInExpo).UpdateType(UpdateType.TimeScaleIndependentUpdate).OnComplete(delegate () {
        //        isAnimation = false;
        //        slowTween = null;
        //    }));
        //}
    }

    public void SetServerTimeNow(DateTime server_time)
    {
        serverTime = server_time.Ticks;
        lastLocalTime = DateTime.Now.Ticks;
    }


    public static long GetUnixTimestamp(DateTime dateTime)
    {
        TimeSpan timeSpan = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));
        return (long)timeSpan.TotalMilliseconds;
    }
}
