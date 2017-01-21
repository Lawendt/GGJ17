using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //[System.NonSerialized]
    //public Animator animator;
    public GameObject gameplayUI;
    public Renderer wavePlane;
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

    public List<EnemyType> queueInteraction;
    void Update()
    {
        debugCurrentEnemy.text = currentEnemy.ToString();
        fill.fillAmount = life / (float)maxLife;
        percentage.text = life.ToString("F1") + "%";
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            queueInteraction.Add(EnemyType.Punk);
            StartPlaying(EnemyType.Punk); //Tocando Punk
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            queueInteraction.Add(EnemyType.Classic);
            StartPlaying(EnemyType.Classic); //Tocando Clássico
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            queueInteraction.Add(EnemyType.Reggae);
            StartPlaying(EnemyType.Reggae); //Tocando Reggae
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            queueInteraction.Add(EnemyType.Eletronic);
            StartPlaying(EnemyType.Eletronic); //Tocando Eletrônica
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            KeyUp(EnemyType.Punk);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            KeyUp(EnemyType.Classic);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            KeyUp(EnemyType.Reggae);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            KeyUp(EnemyType.Eletronic);

        }

    }

    public void KeyUp(EnemyType type)
    {
        queueInteraction.Remove(type);
        if (currentEnemy == type && queueInteraction.Count != 0)
        {
            StartPlaying(queueInteraction[queueInteraction.Count - 1]);
        }
        else
        {
            StopPlaying(type); //Tocando Punk
        }

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
        Debug.Log("Test");
        wavePlane.material.SetVector("_SonarWaveColor", new Color(0.1f, 1, 0.1f, 0));
        // Matar inimigos
    }
    public void PlayClassic()
    {

        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
        wavePlane.material.SetVector("_SonarWaveColor", new Color(1, 0.1f, 0.1f, 0));
        // Matar inimigos

    }
    public void PlayReggae()
    {

        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
        wavePlane.material.SetVector("_SonarWaveColor", new Color(1.0f, 1.0f, 0.1f, 0));
        // Matar inimigos
    }
    public void PlayEletronic()
    {
        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
        wavePlane.material.SetVector("_SonarWaveColor", new Color(0.1f, 0.1f, 1.0f, 0));
        // Matar inimigos
        // Matar inimigos
    }

    void StopPlaying(EnemyType type)
    {
        wavePlane.material.SetVector("_SonarWaveColor", new Color(1.0f, 1.0f, 1.0f, 0));
        if (currentEnemy == type)
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
