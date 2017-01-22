using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Restart()
    {
        
    }
    public void LoadingScene(string Name)
    {
        SceneManager.LoadSceneAsync(Name);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
