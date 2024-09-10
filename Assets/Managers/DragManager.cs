using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;
    [SerializeField] GameObject dragObject;
    [SerializeField] float defaultClickableRadius;
    [SerializeField] float clickableRadius;
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
        if (PauseManager.Instance.IsPaused)
            return;

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

        // Scale the clickable radius based on the camera size
        if (MatterManager.Instance && MatterManager.Instance.LargestMatter && MatterManager.Instance.LargestMatter.GetComponent<Matter>())
            clickableRadius = defaultClickableRadius + MatterManager.Instance.LargestMatter.GetComponent<Matter>().fusions * 0.1f;
        else
            clickableRadius = defaultClickableRadius;

        // Check for particles within the scaled clickable radius
        Collider2D collider = Physics2D.OverlapCircle(targetPosition, clickableRadius);
        if (collider && collider.GetComponent<Matter>())
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