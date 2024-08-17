using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }

    // Game
    public bool PauseInput { get; private set; }
    public Vector2 MousePositionInput { get; private set; }
    public bool LeftMouseInput { get; private set; }

    // UI
    public bool UnpauseInput { get; private set; }

    // Action maps
    InputActionMap gameMap;
    InputActionMap uiMap;


    // Player actions
    InputAction pauseAction;
    InputAction mousePositionAction;
    InputAction leftMouseAction;

    // UI actions
    InputAction unpauseAction;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // Get reference to player input
        PlayerInput = GetComponent<PlayerInput>();
        if (PlayerInput == null) Debug.Log("Player Input not found!");

        // Get references to action maps
        gameMap = PlayerInput.actions.FindActionMap("Game", true);
        uiMap = PlayerInput.actions.FindActionMap("UI", true);

        InitializeInputActions();
    }

    void InitializeInputActions()
    {
        // Game actions
        pauseAction = gameMap.FindAction("Pause", true);
        mousePositionAction = gameMap.FindAction("Mouse Position", true);
        leftMouseAction = gameMap.FindAction("Left Mouse", true);

        // UI actions
        unpauseAction = uiMap.FindAction("Unpause", true);
    }

    void Update()
    {
        UpdateInputs();
    }

    void UpdateInputs()
    {
        // Player inputs
        PauseInput = pauseAction.WasPressedThisFrame();
        MousePositionInput = mousePositionAction.ReadValue<Vector2>();
        LeftMouseInput = leftMouseAction.IsPressed();

        // UI Inputs
        UnpauseInput = unpauseAction.WasPressedThisFrame();
    }
}
