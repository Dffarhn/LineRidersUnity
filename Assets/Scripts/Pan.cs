using UnityEngine;

public class Pan : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 2f;

    private Transform mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    public Vector2 PanDirection(Vector2 mouseScreenPosition)
    {
        Vector2 direction = Vector2.zero;

        // Check for the top part of the screen
        if (mouseScreenPosition.y >= Screen.height * 0.95f)
        {
            direction.y += 1;
        }
        // Check for the bottom part of the screen
        else if (mouseScreenPosition.y <= Screen.height * 0.05f)
        {
            direction.y -= 1;
        }

        // Check for the right part of the screen
        if (mouseScreenPosition.x >= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        // Check for the left part of the screen
        else if (mouseScreenPosition.x <= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    public void PanScreen(Vector2 mouseScreenPosition)
    {
        Vector2 direction = PanDirection(mouseScreenPosition);

        // Convert the direction to world space and move the camera
        Vector3 moveDirection = new Vector3(direction.x, direction.y, 0f); // Make sure the z-value is 0
        mainCamera.position = Vector3.Lerp(mainCamera.position, mainCamera.position + moveDirection, Time.deltaTime * panSpeed);
    }
}
