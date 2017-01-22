using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Singleton<Player>
{
    //[System.NonSerialized]
    //public Animator animator;

    public WaveGenerator soundWaves;
    [Range(0f, 100f)]
    public float life = 100f;
    public int maxLife = 100;
    public Image fill;
    public Text percentage;
    public GameObject guitar, keyboard, techno, drums, idle;
    public GameObject guitarOBJ, keyboardOBJ, technoOBJ, drumsOBJ;
    public GameObject rainPrefab;
    public LoadScene sceneManager;
    public Text scoreText;
    public float score = 0;
    public WinnerManager winner;
    public int numberOfPeopleShaking = 0;

    private EnemyManager enemyManager;
    ParticleSystem rain;
    public EnemyType currentEnemy;
    // public Text debugCurrentEnemy;

    public float loseByPerson;
    public float earnByDollar;

    float waveSize = 1f;
    
    void Start()
    {
        rain = rainPrefab.GetComponent<ParticleSystem>();
        addScore(0);
        life = 50;
        currentEnemy = EnemyType.None;
        enemyManager = EnemyManager.Instance;
        //Animator = GetComponent<Animator>();


        guitar.SetActive(false);
        keyboard.SetActive(false);
        techno.SetActive(false);
        drums.SetActive(false);


    }

    bool pressed = false;
    public List<EnemyType> queueInteraction;
    void Update()
    {
        // debugCurrentEnemy.text = currentEnemy.ToString();
        fill.fillAmount = life / (float)maxLife;
        percentage.text = life.ToString("F1") + "%";

       

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {

            KeyUp(EnemyType.Classic);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            KeyUp(EnemyType.Eletronic);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            KeyUp(EnemyType.Punk);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            KeyUp(EnemyType.Reggae);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
            if(waveSize < 5f)
            {
                waveSize += Time.deltaTime * 2f;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pressed = true;
            queueInteraction.Add(EnemyType.Classic);
            StartPlaying(EnemyType.Classic); //Tocando Clássico
            MusicManager.Instance.ChangeMusicType(EnemyType.Classic);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pressed = true;
            queueInteraction.Add(EnemyType.Eletronic);
            StartPlaying(EnemyType.Eletronic); //Tocando Eletronica
            MusicManager.Instance.ChangeMusicType(EnemyType.Eletronic);

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            pressed = true;
            queueInteraction.Add(EnemyType.Punk);
            StartPlaying(EnemyType.Punk); //Tocando Punk
            MusicManager.Instance.ChangeMusicType(EnemyType.Punk);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pressed = true;
            queueInteraction.Add(EnemyType.Reggae);
            StartPlaying(EnemyType.Reggae); //Tocando Reggae
            MusicManager.Instance.ChangeMusicType(EnemyType.Reggae);
        }
        

        soundWaves.size = waveSize;
        if (numberOfPeopleShaking != 0)
        {
            life -= numberOfPeopleShaking * Time.deltaTime;
        }
        if (life < maxLife / 2)
        {
            if (!rain.isPlaying)
            {
                rain.Play();
            }

            if (life <= 0f) //LOSS
            {
                Time.timeScale = 0f;
                //Detonar particulas do palco
                winner.winner = 1;
                winner.money = score;
                sceneManager.LoadingScene("EndGame");
            }
        }
        else
        {
            if (rain.isPlaying)
            {
                rain.Stop();

            }
        }
        //// Win by Score
        //if (score >= 1000.0f) //WIN
        //{
        //    Time.timeScale = 0f;
        //    //Fade
        //    winner.winner = 0;
        //    winner.money = score;
        //    sceneManager.LoadingScene("EndGame");

        //}


        if (Time.timeSinceLevelLoad >= 90) //WIN
        {
            Time.timeScale = 0f;
            //Fade
            winner.winner = 0;
            winner.money = score;
            sceneManager.LoadingScene("EndGame");

        }
    }

    public void KeyUp(EnemyType type)
    {
        waveSize = 1f;
        pressed = false;
        queueInteraction.Remove(type);
        if (currentEnemy == type && queueInteraction.Count != 0)
        {
            pressed = true;
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
        idle.SetActive(false);
        guitar.SetActive(true);
        guitarOBJ.SetActive(false);

        Debug.Log("Test");
        soundWaves.waveType = EnemyType.Punk;

        // Matar inimigos
    }
    public void PlayClassic()
    {
        idle.SetActive(false);
        keyboard.SetActive(true);
        keyboardOBJ.SetActive(false);

        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
        soundWaves.waveType = EnemyType.Classic;
        // Matar inimigos

    }
    public void PlayReggae()
    {
        idle.SetActive(false);
        drums.SetActive(true);
        drumsOBJ.SetActive(false);

        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
        soundWaves.waveType = EnemyType.Reggae;
        // Matar inimigos
    }
    public void PlayEletronic()
    {
        idle.SetActive(false);
        techno.SetActive(true);
        technoOBJ.SetActive(false);


        //Desativar onda
        //Audio
        //Animação
        //Instanciar nova onda
        soundWaves.waveType = EnemyType.Eletronic;
        // Matar inimigos
        // Matar inimigos
    }

    void StopPlaying(EnemyType type)
    {
        if (currentEnemy == type)
        {
            //Start base music
            MusicManager.Instance.ChangeMusicType(EnemyType.None);

            switch (type)
            {
                case EnemyType.Classic:
                    keyboard.SetActive(false);
                    keyboardOBJ.SetActive(true);
                    break;
                case EnemyType.Punk:
                    guitar.SetActive(false);
                    guitarOBJ.SetActive(true);
                    break;
                case EnemyType.Reggae:
                    drums.SetActive(false);
                    drumsOBJ.SetActive(true);
                    break;
                case EnemyType.Eletronic:
                    techno.SetActive(false);
                    technoOBJ.SetActive(true);
                    break;
            }
            soundWaves.waveType = EnemyType.None;
            enemyManager.StopPlaying(type);
            currentEnemy = EnemyType.None;
            idle.SetActive(true);
        }
    }
    void Idle()
    {
        currentEnemy = EnemyType.None;
        //Desativar onda
    }

    public void addScore(float f)
    {
        score += f;
        scoreText.text = "$" + score.ToString("0.00");
        life += f * earnByDollar;
        if (life > maxLife)
        {
            life = maxLife;
        }
    }

    #region Shake
    bool isFixingShake;
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            // Debug.Log(collider.name);
            if (!collider.GetComponent<EnemyStandardBehaviour>().shaking)
            {
                collider.GetComponent<EnemyStandardBehaviour>().Shake();
                AddShake();
            }
        }
    }
    public void AddShake()
    {
        //if (isFixingShake)
        //{
        //    StopCoroutine("_fixShake");
        //}
        numberOfPeopleShaking++;
        if (numberOfPeopleShaking == 1)
            StartCoroutine("_shake");
    }
    public void removeShake()
    {
        numberOfPeopleShaking--;
        if (numberOfPeopleShaking == 0)
            StopCoroutine("_shake");

        //StartCoroutine("_fixShake");
    }

    IEnumerator _shake()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        Vector3 dest = new Vector3();

        dest.z = -10 * numberOfPeopleShaking / 3.0f;
        while (numberOfPeopleShaking > 0)
        {
            // Do transition from rot to dest based on lerp.
            while (Vector3.Distance(rot, dest) > 0.1)
            {
                rot = Vector3.Lerp(rot, dest, 0.1f);
                transform.rotation = Quaternion.Euler(rot);
                yield return new WaitForEndOfFrame();
            }
            // After transition is ended, change the target side. Check wether is positive/negative
            if (dest.z > 0)
            {
                dest.z = -10 * numberOfPeopleShaking / 3.0f;
                if (dest.z < -25)
                {
                    dest.z = -25;
                }
            }
            else
            {
                dest.z = 10 * numberOfPeopleShaking / 3.0f;
                if (dest.z > 25)
                {
                    dest.z = 25;
                }
            }
        }
    }

    IEnumerator _fixShake()
    {
        yield return new WaitForSeconds(2);
        isFixingShake = true;
        Vector3 rot = transform.rotation.eulerAngles;
        Vector3 dest = new Vector3();
        while (Vector3.Distance(rot, dest) > 0.1)
        {
            rot = Vector3.Lerp(rot, dest, 0.3f);
            transform.rotation = Quaternion.Euler(rot);
            yield return new WaitForEndOfFrame();
        }

        isFixingShake = false;

    }

    #endregion
}
