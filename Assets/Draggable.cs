using UnityEngine;

public class Draggable : MonoBehaviour
{
    bool isDragging = false;
    float clickableRadius = 0.1f;
    Vector3 mousePosition;

    void Update()
    {
        HandleInput();
        HandleMovement();
    }

    void HandleInput()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            OnMouseDown();

        if (Input.GetMouseButtonUp(0))
            OnMouseUp();
    }

    void HandleMovement()
    {
        if (!isDragging)
            return;

        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    void OnMouseDown()
    {
        if (isDragging)
            return;

        // Check if the mouse is within the clickable radius of the particle
        Collider2D collider = Physics2D.OverlapCircle(mousePosition, clickableRadius);
        if (collider != null && collider.gameObject == gameObject)
        {
            isDragging = true;
        }
    }

    void OnMouseUp()
    {
        if (!isDragging)
            return;

        isDragging = false;
    }
}
