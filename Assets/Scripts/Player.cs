using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //[System.NonSerialized]
    //public Animator animator;
    public GameObject gameplayUI;
    [Range(0f, 100f)]
    public float life = 100f;
    public int maxLife = 100;
    Image fill;
    Text percentage;

    private EnemyManager enemyManager;
    public EnemyType currentEnemy;
    public Text debugCurrentEnemy;
    void Start()
    {
        life = (float)maxLife;
        currentEnemy = EnemyType.None;
        enemyManager = EnemyManager.Instance;
        //Animator = GetComponent<Animator>();
        fill = gameplayUI.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        percentage = gameplayUI.transform.GetChild(0).GetChild(1).GetComponent<Text>();
    }
    void Update()
    {
        debugCurrentEnemy.text = currentEnemy.ToString();
        fill.fillAmount = life/(float)maxLife;
        percentage.text = life.ToString("F1") + "%";
        if (Input.GetKeyDown(KeyCode.UpArrow))
            StartPlaying(EnemyType.Punk); //Tocando Punk
        if (Input.GetKeyDown(KeyCode.DownArrow))
            StartPlaying(EnemyType.Classic); //Tocando Clássico
        if (Input.GetKeyDown(KeyCode.RightArrow))
            StartPlaying(EnemyType.Reggae); //Tocando Reggae
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            StartPlaying(EnemyType.Eletronic); //Tocando Eletrônica

        if (Input.GetKeyUp(KeyCode.UpArrow))
            StopPlaying(EnemyType.Punk); //Tocando Punk
        if (Input.GetKeyUp(KeyCode.DownArrow))
            StopPlaying(EnemyType.Classic); //Tocando Clássico
        if (Input.GetKeyUp(KeyCode.RightArrow))
            StopPlaying(EnemyType.Reggae); //Tocando Reggae
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            StopPlaying(EnemyType.Eletronic); //Tocando Eletrônica


    }

    public void StartPlaying(EnemyType type)
    {
        if (currentEnemy != EnemyType.None)
        {
            StopPlaying(currentEnemy);
        }
        currentEnemy = type;
        enemyManager.PlayFor(currentEnemy);
        switch (type)
        {
            case EnemyType.Classic:
                PlayClassic();
                break;
            case EnemyType.Punk:
                PlayPunk();
                break;
            case EnemyType.Reggae:
                PlayReggae();
                break;
            case EnemyType.Eletronic:
                PlayEletronic();
                break;
        }
    }
    public void PlayPunk()
    {
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda

        // Matar inimigos
    }
    public void PlayClassic()
    {
        
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda

        // Matar inimigos

    }
    public void PlayReggae()
    {

        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda

        // Matar inimigos
    }
    public void PlayEletronic()
    {
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda

        // Matar inimigos
    }

    void StopPlaying(EnemyType type)
    {
        if(currentEnemy == type)
        {
        enemyManager.StopPlaying(type);
            currentEnemy = EnemyType.None;
        }
    }
    void Idle()
    {
        currentEnemy = EnemyType.None;
        //Desativar onda
    }
}
