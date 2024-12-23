using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField]
    private float zoomSpeed = 2f;

    [SerializeField]
    private float zoomInMax = 1f;

    [SerializeField]
    private float zoomOutMax = 15f;

    private Camera mainCamera; // Change CameraFollow to Camera if you're only dealing with the Camera component

    private float startingZPosition;

    private void Awake()
    {
        mainCamera = Camera.main; // Get the Camera component

        startingZPosition = mainCamera.transform.position.z;
    }

    public void ZoomScreen(float increment)
    {
        if (increment == 0)
            return;

        // Calculate the target orthographic size, ensuring it's within the zoom limits
        float target = Mathf.Clamp(mainCamera.orthographicSize + increment, zoomInMax, zoomOutMax);

        // Smoothly interpolate towards the target orthographic size
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            target,
            Time.deltaTime * zoomSpeed
        );
    }
}
