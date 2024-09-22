using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject howToPlay;
    public void LoadLevel()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void HowToPlay()
    {
        howToPlay.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Back()
    {
        howToPlay.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
