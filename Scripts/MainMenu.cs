using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public void playGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void pauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

    }
    public void continueGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
