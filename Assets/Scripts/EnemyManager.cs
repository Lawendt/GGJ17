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
    public enum TypeGeneration
    {
        readFromList,
        random
    }
    public enum TypeDetection
    {
        justFirst,
        distanceReach,
        everyoneDances
    }

    public TypeGeneration typeGeneration;
    public TypeDetection typeDetection;

    float timeStart;
    public List<EnemyInstance> enemy;
    public GameObject enemyPrefab;
    public List<EnemyStandardBehaviour> enemyInScene;

    public float distanceToBeAffected;
    public float timetoWaitToEnjoy = 1f;

    public float minRandom, maxRandom;


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
        switch (typeGeneration)
        {
            case TypeGeneration.readFromList:
                while (i < enemy.Count)
                {
                    if (enemy[i].time != 0)
                    {
                        yield return new WaitForSeconds(enemy[i].time);
                    }
                    InstanceEnemy(enemy[i].type, i);
                    i++;
                }
                break;
            case TypeGeneration.random:
                while (true)
                {
                    float time = UnityEngine.Random.Range(minRandom * 100, maxRandom * 100) / 100;
                    yield return new WaitForSeconds(time);
                    InstanceEnemy((EnemyType)UnityEngine.Random.Range(0, 4), i);
                    i++;

                }
                break;

        }

    }

    public void InstanceEnemy(EnemyType type, int i)
    {
        GameObject e = Instantiate(enemyPrefab);
        e.name = type.ToString() + " " + i  + " @ " + Time.time;
        EnemyStandardBehaviour es = e.GetComponent<EnemyStandardBehaviour>();

        float a = UnityEngine.Random.Range(-45, 45);
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            a += 180;
        }

        es.Initialize(a, 12, 1, type);
        enemyInScene.Add(es);
        //e.GetComponent<SpriteRenderer>().color = new Color(i / 5.0f, 0,0);
    }

    public Coroutine coroutine;
    public void PlayFor(EnemyType type)
    {
        coroutine = StartCoroutine(PlayForUpdate(type));
    }

    public IEnumerator PlayForUpdate(EnemyType type)
    {
        float time = timetoWaitToEnjoy;
        while (true)
        {
        float lastDistance = -1;
            for (int i = 0; i < enemyInScene.Count; i++)
            {
                bool _do = true;
                switch (typeDetection)
                {
                    case TypeDetection.distanceReach:
                        _do &= enemyInScene[i].distanceFromObjective() <= distanceToBeAffected;
                        break;
                    case TypeDetection.everyoneDances:
                        _do &= true;
                        break;
                    case TypeDetection.justFirst:
                        if (lastDistance == -1)
                        {
                            _do = true;
                            lastDistance = enemyInScene[i].distanceFromObjective();
                        }
                        else if (lastDistance >= enemyInScene[i].distanceFromObjective())
                        {
                            lastDistance = enemyInScene[i].distanceFromObjective();
                            _do = true;
                        }
                        else
                        {
                            //if (enemyInScene[i].type == type)
                            //{
                            //    Debug.Log("Not Close Enough");
                            //}
                            _do = false;
                        }
                        break;

                }
                #region Debug
                //if (enemyInScene[i].receivedEnjoy)
                //    Debug.Log("Already received");
                #endregion
                if (enemyInScene[i].type == type && !enemyInScene[i].receivedEnjoy && _do)
                {
                    enemyInScene[i].Enjoy(time);
                }
                time -= Time.deltaTime;
                if (time < 0)
                    time = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopPlaying(EnemyType type)
    {
        StopCoroutine(coroutine);
        for (int i = 0; i < enemyInScene.Count; i++)
        {
            if (enemyInScene[i].type == type)
            {
                enemyInScene[i].StopEnjoying(timetoWaitToEnjoy);
            }
        }
    }

    public void removeEnemy(EnemyStandardBehaviour e)
    {
        enemyInScene.Remove(e);
    }
}
