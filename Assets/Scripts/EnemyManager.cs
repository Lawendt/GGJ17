using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EnemyType
{
    Classic,
    Punk,
    Reggae,
    Eletronic
}

[Serializable]
public class EnemyInstance
{
    public EnemyType type;
    public float time;
}

public class EnemyManager : MonoBehaviour
{
    public
    float timeStart;
    public List<EnemyInstance> enemy;
    public GameObject enemyPrefab;

    // Use this for initialization
    void Start()
    {
        Play();
    }

    public void Play()
    {
        timeStart = Time.time;
        StartCoroutine(run());
    }

    IEnumerator run()
    {
        int i = 0;
        while (i < enemy.Count)
        {
            if (enemy[i].time != 0)
            {
                yield return new WaitForSeconds(enemy[i].time);
            }
            InstanceEnemy(enemy[i].type, i );
            i++;
        }
    }


    public void InstanceEnemy(EnemyType type, int i)
    {
        GameObject e = Instantiate(enemyPrefab);
        e.name = "Enemy " + " i " + " @ " + Time.time;
        e.GetComponent<EnemyStandardBehaviour>().Initialize(UnityEngine.Random.Range(0, 360), 12, 1);
        //e.GetComponent<SpriteRenderer>().color = new Color(i / 5.0f, 0,0);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
