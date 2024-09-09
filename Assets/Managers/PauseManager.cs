using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] private GameObject pauseMenu;
    private GameObject menuInstance;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (InputManager.Instance.PauseInput || InputManager.Instance.UnpauseInput)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (IsPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (IsPaused) return;

        IsPaused = true;
        Time.timeScale = 0;
        InputManager.PlayerInput.SwitchCurrentActionMap("UI");

        if (pauseMenu)
        {
            menuInstance = Instantiate(pauseMenu, transform.position, Quaternion.identity);
        }
    }

    public void UnPause()
    {
        if (!IsPaused) return;

        IsPaused = false;
        Time.timeScale = 1;
        InputManager.PlayerInput.SwitchCurrentActionMap("Game");

        if (menuInstance != null)
        {
            Destroy(menuInstance);
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Pause();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Pause();
        }
    }
}
