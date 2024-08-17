using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public static void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
