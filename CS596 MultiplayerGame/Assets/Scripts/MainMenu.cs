using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level");
        Debug.Log("Loading Level Success");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
