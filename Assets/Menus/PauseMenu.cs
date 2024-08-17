using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        PauseManager.Instance.UnPause();
        Destroy(transform.root.gameObject);
    }

    public void MainMenu()
    {
        PauseManager.Instance.UnPause();
        GameManager.MainMenu();
    }
}
