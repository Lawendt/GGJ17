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
    Eletronic,
    None
}

[Serializable]
public class EnemyInstance
{
    public EnemyType type;
    public float time;
}

public class EnemyManager : Singleton<EnemyManager>
{
    float timeStart;
    public List<EnemyInstance> enemy;
    public GameObject enemyPrefab;
    public List<EnemyStandardBehaviour> enemyInScene;

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
        EnemyStandardBehaviour es = e.GetComponent<EnemyStandardBehaviour>();
        es.Initialize(UnityEngine.Random.Range(0, 360), 12, 1);
        es.setType(type);
        enemyInScene.Add(es);
        //e.GetComponent<SpriteRenderer>().color = new Color(i / 5.0f, 0,0);
    }
    
    public void PlayFor(EnemyType type)
    {
        for(int i = 0; i < enemyInScene.Count; i++)
        {
            if(enemyInScene[i].type == type)
            {
                enemyInScene[i].Enjoy();
            }
        }
    }

    public void StopPlaying(EnemyType type)
    {
        for (int i = 0; i < enemyInScene.Count; i++)
        {
            if (enemyInScene[i].type == type)
            {
                enemyInScene[i].StopEnjoying();
            }
        }
    }

    public void removeEnemy(EnemyStandardBehaviour e)
    {
        enemyInScene.Remove(e);
    }
}
