using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : StateMachineBase
{
    enum States
    {
        IntroState,
        GamePlayState,
        GameEndState,
    }

    #region ### singleton ###
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    private static bool applicationIsQuitting = false;

    public void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    public void OnDestroy()
    {
        if (applicationIsQuitting) return;
        _instance = null;
    }

    protected override void OnAwake()
    {
        _instance = this;
    }
    #endregion

    public GameUI ui;
    public PlayerControl player;
    private int score = 0;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        score = 0;
        ui.SetScore(score);
        CurrentState = States.IntroState;
    }

    private IEnumerator IntroStateEnter()
    {
        CurrentState = States.GamePlayState;
        yield return null;
    }

    private IEnumerator GamePlayStateEnter()
    {
        bool isTouched = false;
        Vector2 prevTouch = Vector2.zero;
        while (true)
        {
            if (player.hp == 0)
            {
                CurrentState = States.GameEndState;
                break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                isTouched = true;
                prevTouch = Util.GetMousePosition();
            }

            if(Input.GetMouseButtonUp(0))
            {
                isTouched = false;
            }

            if (isTouched)
            {
                Vector2 pos = Util.GetMousePosition();

                float diff = pos.x - prevTouch.x;
                player.SetPos(diff);
                prevTouch = pos;
            }

            yield return null;
        }
    }

    private IEnumerator GameEndStateEnter()
    {
        yield return null;
    }

    public void AddScore(int score)
    {
        this.score += score;
        ui.SetScore(this.score);
    }
}
