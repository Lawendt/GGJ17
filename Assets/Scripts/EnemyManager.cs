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
                    float time = UnityEngine.Random.Range(0, 500) / 100;
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
        e.name = type.ToString() + " i " + " @ " + Time.time;
        EnemyStandardBehaviour es = e.GetComponent<EnemyStandardBehaviour>();
        es.Initialize(UnityEngine.Random.Range(0, 360), 12, 1, type);
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
        float lastDistance = -1;
        while (true)
        {
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
                            _do = false;
                        }
                        break;

                }
                if (enemyInScene[i].type == type && !enemyInScene[i].receivedEnjoy && _do)
                {
                    enemyInScene[i].Enjoy(time);
                }
                time -= Time.deltaTime;
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
