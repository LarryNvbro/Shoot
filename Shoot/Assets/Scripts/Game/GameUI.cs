using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public UILabel lbScore;
    public UILabel lbWave;

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

}
