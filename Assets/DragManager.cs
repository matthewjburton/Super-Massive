using UnityEngine;
using UnityEngine.UIElements;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;
    [SerializeField] GameObject dragObject;
    readonly float clickableRadius = 0.2f;
    Vector3 targetPosition;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
    }

    void HandleInput()
    {
        // Touch input
        if (InputManager.Instance.PrimaryTouchInput)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(InputManager.Instance.PrimaryTouchPositionInput);
            OnInputDown();
        }
        else
        {
            OnInputUp();
        }
    }

    void HandleMovement()
    {
        if (!dragObject)
            return;

        dragObject.transform.position = new Vector3(targetPosition.x, targetPosition.y, dragObject.transform.position.z);
    }

    void OnInputDown()
    {
        if (dragObject)
            return;

        Collider2D collider = Physics2D.OverlapCircle(targetPosition, clickableRadius);
        if (collider && collider.GetComponent<Particle>())
        {
            dragObject = collider.gameObject;
        }
    }

    void OnInputUp()
    {
        if (!dragObject)
            return;

        dragObject = null;
    }
}