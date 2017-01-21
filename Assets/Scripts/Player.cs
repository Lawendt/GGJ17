using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //[System.NonSerialized]
    //public Animator animator;
    public GameObject bar, measurer;
    [Range(0f,1f)]
    public float life = 1f;
    [Range(-1f, 1f)]
    public float moral = 0f;
    Image fill, point;
    void Start()
    {
        //Animator = GetComponent<Animator>();
        fill = bar.transform.GetChild(0).GetComponent<Image>();
        point = measurer.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    void Update()
    {
        fill.fillAmount = life;
        point.GetComponent<RectTransform>().localPosition = new Vector3(moral * measurer.GetComponent<RectTransform>().rect.width/20f,0f,0f);
        if (Input.GetKey(KeyCode.UpArrow))
            PlayPunk(); //Tocando Punk
        else if (Input.GetKey(KeyCode.DownArrow))
            PlayClassic(); //Tocando Clássico
        else if (Input.GetKey(KeyCode.RightArrow))
            PlayReggae(); //Tocando Reggae
        else if (Input.GetKey(KeyCode.LeftArrow))
            PlayEletronic(); //Tocando Eletrônica
        else //Se não estiver fazendo nenhuma ação
            Idle(); //Idle
    }
    public void PlayPunk()
    {
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
    }
    public void PlayClassic()
    {
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
    }
    public void PlayReggae()
    {
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
    }
    public void PlayEletronic()
    {
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
    }
    void Idle()
    {
        //Desativar onda
    }
}
