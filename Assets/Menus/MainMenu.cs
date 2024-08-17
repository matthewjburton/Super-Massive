using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        GameManager.LoadGame();
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
