using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerManager : MonoBehaviour
{
    public int winner;
    public float money;
    bool firstGame;
    void Awake()
    {
        Application.ExternalEval("if(kongregate){kongregate.stats.submit(\"trollolusque\", " + PlayerPrefs.GetFloat("Money") + ");}");
        firstGame = true;
        DontDestroyOnLoad(gameObject);
    }
    void OnLevelWasLoaded()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "EndGame")
        {
            Time.timeScale = 1f;
            ActiveScene();
            firstGame = false;
        }
        else if (scene.name == "Game")
        {
            if (!firstGame)
                Destroy(gameObject);
        }


    }
    void ActiveScene()
    {
        PlayerPrefs.SetFloat("Money", money);
        Application.ExternalEval("if(kongregate){kongregate.stats.submit(\"trollolusque\", " + PlayerPrefs.GetFloat("Money") + ");}");
        EndSceneManager end = GameObject.Find("EndSceneManager").GetComponent<EndSceneManager>();
        end.value = winner;
        end.score = money;
    }
}