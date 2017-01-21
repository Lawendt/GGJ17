using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[System.NonSerialized]
    //public Animator animator;
    public bool usingKeyboard = false;
    public float life = 5f;
    void Start()
    {
        //Animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            usingKeyboard = true;
            PlayPunk(); //Tocando Punk
        }
        else if (Input.GetKey(KeyCode.DownArrow)) 
        {
            usingKeyboard = true;
            PlayClassic(); //Tocando Clássico
        }
        else if (Input.GetKey(KeyCode.RightArrow)) 
        {
            usingKeyboard = true;
            PlayReggae(); //Tocando Reggae
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            usingKeyboard = true;
            PlayEletronic(); //Tocando Eletrônica
        }
        else //Idle
        {
            usingKeyboard = false;
        }
    }
    public void PlayPunk()
    {
    }
    public void PlayClassic()
    {
    }
    public void PlayReggae()
    {
    }
    public void PlayEletronic()
    {
    }
}
