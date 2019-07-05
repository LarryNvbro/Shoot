using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    public UILabel lbScore;
    public UILabel lbWave;
    public UISlider hpGauge;

    public void SetScore(int score)
    {
        lbScore.text = string.Format("Score: {0}", score);
    }

    public void SetGameOver(bool isOver)
    {
        transform.Find("game_over").gameObject.SetActive(isOver);
    }

    public void SetWave(int wave)
    {
        lbWave.text = string.Format("Wave {0}", wave);
    }

    public void SetPlayerHp(int hp, int maxHp)
    {
        float value = (float)hp / maxHp;
        DOTween.To(() => hpGauge.value, x => hpGauge.value = x, value, 0.2f);
    }

}
