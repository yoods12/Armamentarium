using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayGame");
    }
    public void Gear()
    {
        SceneManager.LoadScene("Gear");
    }
    public void QuitGame()
    {
        print("Quit");
        Application.Quit();
    }
}
