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

    public GameObject player;
    public Player playerScript;

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
        playerScript = player.GetComponent<Player>();
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
                  //  i %= enemy.Count;
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
        e.name = type.ToString() + " " + i + " @ " + Time.time;
        EnemyStandardBehaviour es = e.GetComponent<EnemyStandardBehaviour>();
        es.center = player.transform.position;
        float a = UnityEngine.Random.Range(-40, 40);
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            a += 180;
        }

        es.Initialize(a, 12, 1, type);
        //if (enemyInScene.Count == 0)
        //    es.SetHighlight(true);
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
        float lastDistance;
        while (true)
        {
            lastDistance = -1;
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

                if (_do)
                {
                    if (enemyInScene[i].type == type && !enemyInScene[i].receivedEnjoy) // If the enemy has to enjoy and isn't alreading enjoying
                    {
                        enemyInScene[i].Enjoy(time);
                    }
                    else if (IsHating(enemyInScene[i].type, type) && !enemyInScene[i].hating)
                    {
                        enemyInScene[i].Hate(time);
                    }
                    else
                    {
                        enemyInScene[i].Confuse();
                    }

                }
                time -= Time.deltaTime;
                if (time < 0)
                    time = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public bool IsHating(EnemyType typeEnemy, EnemyType current)
    {
        if (typeEnemy == EnemyType.Classic)
        {
            return current == EnemyType.Eletronic;
        }
        else if (typeEnemy == EnemyType.Eletronic)
        {
            return current == EnemyType.Reggae;
        }
        else if (typeEnemy == EnemyType.Reggae)
        {
            return current == EnemyType.Punk;
        }
        else if (typeEnemy == EnemyType.Punk)
        {
            return current == EnemyType.Classic;
        }
        else
        {
            return false;
        }
    }

    public void StopPlaying(EnemyType type)
    {
        StopCoroutine(coroutine);
        for (int i = 0; i < enemyInScene.Count; i++)
        {
            enemyInScene[i].EndConfuse();
            enemyInScene[i].StopHating();
            if (enemyInScene[i].type == type)
            {
                enemyInScene[i].StopEnjoying(timetoWaitToEnjoy);
            }
        }
    }

    public void removeEnemy(EnemyStandardBehaviour e)
    {
        enemyInScene.Remove(e);
        //if (enemyInScene.Count != 0)
        //    enemyInScene[0].SetHighlight(true);
    }


}
