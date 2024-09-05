using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.iOS;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }

    public static SystemGestureDeferMode deferSystemGesturesMode;

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
        MousePositionInput = mousePositionAction.ReadValue<Vector2>();
        LeftMouseInput = leftMouseAction.IsPressed();

        // Skip Cutscene input (Mouse or Tap)
        bool tapDetected = false;

        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    tapDetected = true;
                    break;
                }
            }
        }
        SkipCutsceneInput = skipCutsceneAction.IsPressed() || tapDetected;

        // Pause input (Escape or Two-Finger Tap)
        bool twoFingerTap = false;

        if (Touchscreen.current != null)
        {
            var touchCount = Touchscreen.current.touches.Count;
            if (touchCount == 2)
            {
                var firstTouch = Touchscreen.current.touches[0];
                var secondTouch = Touchscreen.current.touches[1];

                if (firstTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began &&
                    secondTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    twoFingerTap = true;
                }
            }
        }
        PauseInput = pauseAction.WasPressedThisFrame() || twoFingerTap;

        // Primary Touch input
        if (Touchscreen.current != null)
        {
            var primaryTouch = Touchscreen.current.primaryTouch;

            if (primaryTouch.isInProgress)
            {
                PrimaryTouchInput = true;
                PrimaryTouchPositionInput = primaryTouch.position.ReadValue();
            }
            else
            {
                PrimaryTouchInput = false;
            }
        }
        else
        {
            PrimaryTouchInput = false;
        }

        // UI Inputs
        UnpauseInput = unpauseAction.WasPressedThisFrame() || twoFingerTap;
    }
}
