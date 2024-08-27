using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }

    public enum ControlScheme { Mouse, Touch }
    public ControlScheme CurrentControlScheme { get; private set; }

    // Game
    public bool PauseInput { get; private set; }
    public Vector2 MousePositionInput { get; private set; }
    public bool LeftMouseInput { get; private set; }
    public bool SkipCutsceneInput { get; private set; }
    public bool PrimaryTouchInput { get; private set; }
    public Vector2 PrimaryTouchPositionInput { get; private set; }

    // UI
    public bool UnpauseInput { get; private set; }

    // Action maps
    InputActionMap gameMap;
    InputActionMap uiMap;


    // Player actions
    InputAction pauseAction;
    InputAction mousePositionAction;
    InputAction leftMouseAction;
    InputAction skipCutsceneAction;
    InputAction primaryTouchAction;

    // UI actions
    InputAction unpauseAction;

    void Awake()
    {
        TouchSimulation.Enable();

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
        skipCutsceneAction = gameMap.FindAction("Skip Cutscene", true);
        primaryTouchAction = gameMap.FindAction("Primary Touch", true);

        // UI actions
        unpauseAction = uiMap.FindAction("Unpause", true);
    }

    void Update()
    {
        UpdateInputs();
        UpdateControlScheme();
    }

    void UpdateInputs()
    {
        // Player inputs
        PauseInput = pauseAction.WasPressedThisFrame();
        MousePositionInput = mousePositionAction.ReadValue<Vector2>();
        LeftMouseInput = leftMouseAction.IsPressed();
        SkipCutsceneInput = skipCutsceneAction.IsPressed();
        PrimaryTouchInput = primaryTouchAction.IsPressed();
        PrimaryTouchPositionInput = primaryTouchAction.ReadValue<Vector2>();

        // Touch input
        /*if (primaryTouchAction.triggered)
        {
            PrimaryTouchInput = true;
            PrimaryTouchPositionInput = primaryTouchAction.ReadValue<Vector2>();
        }
        else
        {
            PrimaryTouchInput = false;
        }*/

        // UI Inputs
        UnpauseInput = unpauseAction.WasPressedThisFrame();
    }

    void UpdateControlScheme()
    {
        // Check for mouse input
        /*if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        {
            CurrentControlScheme = ControlScheme.Mouse;
        }*/

        // Check for touch input
        if (Touchscreen.current != null && Touchscreen.current.wasUpdatedThisFrame)
        {
            CurrentControlScheme = ControlScheme.Touch;
        }
    }
}
