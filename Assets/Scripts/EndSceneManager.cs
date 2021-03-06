﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    public Text mensage;
    public GameObject win, loss;
    [System.NonSerialized]
    public int value;
    [System.NonSerialized]
    public float score;
    void Start()
    {
        ActiveScene();
    }

    public void ActiveScene()
    {
        if (value == 0) //WINNER
        {
            win.SetActive(true);
            mensage.text = "WINNER $"+ score.ToString("0.00") + "!";
        }
        else //LOSSER
        {
           loss.SetActive(true);
            mensage.text = "LOSER $" + score.ToString("0.00") + "!";
        }
    }
}
