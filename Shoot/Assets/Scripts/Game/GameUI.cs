﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public UILabel lbScore;

    void Start()
    {

    }

    public void SetScore(int score)
    {
        lbScore.text = string.Format("Score: {0}", score);
    }

}
