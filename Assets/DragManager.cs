using UnityEngine;
using UnityEngine.UIElements;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;
    [SerializeField] GameObject dragObject;
    readonly float clickableRadius = 0.1f;
    Vector3 mousePosition;

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
        mousePosition = Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePositionInput);

        if (InputManager.Instance.LeftMouseInput)
            OnMouseDown();

        if (!InputManager.Instance.LeftMouseInput)
            OnMouseUp();
    }

    void HandleMovement()
    {
        if (!dragObject)
            return;

        dragObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }

    void OnMouseDown()
    {
        if (dragObject)
            return;

        Collider2D collider = Physics2D.OverlapCircle(mousePosition, clickableRadius);
        if (collider && collider.GetComponent<Particle>())
        {
            dragObject = collider.gameObject;
        }
    }

    void OnMouseUp()
    {
        if (!dragObject)
            return;

        dragObject = null;
    }
}