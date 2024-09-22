using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("tamionette");
    }

    public void HowToPlay()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
