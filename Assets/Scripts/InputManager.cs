using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance{

        get {
            return _instance;
        }

    }


    #region Event
    public delegate void StartDraw();
    public static event StartDraw OnStartDraw;
    public delegate void EndDraw();
    public static event StartDraw OnEndDraw;
    public delegate void StartErase();
    public static event StartDraw OnStartErase;
    public delegate void EndErase();
    public static event StartDraw OnEndErase;
    public delegate void PressedPlay();
    public static event PressedPlay OnPressPlay;

    #endregion
    private MouseControls mouseControls;
    private PlayerControls playerControls;

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }else{
            _instance = this;
        }
        mouseControls = new MouseControls();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        // Enable input controls
        if (mouseControls != null)
        {
            mouseControls.Enable();
            playerControls.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable input controls
        if (mouseControls != null)
        {
            mouseControls.Disable();
            playerControls.Disable();
        }
    }

    void Start()
    {
        mouseControls.Mouse.Click.started += _ =>
        {
            if (OnStartDraw != null)
                OnStartDraw();
        };
        mouseControls.Mouse.Click.canceled += _ =>
        {
            if (OnEndDraw != null)
                OnEndDraw();
        };
        mouseControls.Mouse.Erase.started += _ =>
        {
            if (OnStartErase != null)
                OnStartErase();
        };
        mouseControls.Mouse.Erase.canceled += _ =>
        {
            if (OnEndErase != null)
                OnEndErase();
        };

        playerControls.Player.Space.performed += _ => { if (OnPressPlay != null) OnPressPlay(); };

        Cursor.lockState = CursorLockMode.Confined;
    }

    public float GetZoom()
    {
        return mouseControls.Mouse.Zoom.ReadValue<float>();
    }

    public Vector2 GetMousePosisition()
    {
        return mouseControls.Mouse.Posisition.ReadValue<Vector2>();
    }

    void Update()
    {
        // Your update logic here
    }
}
