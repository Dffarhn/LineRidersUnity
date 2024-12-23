using TMPro; // Import TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class Player : MonoBehaviour
{
    public bool playing { get; private set; }
    private Rigidbody2D rb;
    private CameraFollow cameraFollow;

    [Header("Player Name Display")]
    [SerializeField]
    private TextMeshProUGUI nameTextUI;

    [SerializeField]
    private TextMeshPro nameText3D;

    [SerializeField]
    private TextMeshProUGUI scoreTextUI;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private InputManager inputManager;

    private float timePlaying;
    private bool gameStarted;

    [Header("Game Over Settings")]
    [SerializeField]
    private float maxYDifference = 4f;

    [SerializeField]
    private string gameOverSceneName = "GameOverScene"; // Set the Game Over Scene name

    [Header("Audio Settings")]
    [SerializeField]
    private AudioSource backgroundMusic; // Reference to AudioSource

    [Header("Dynamic Head Line Settings")]
    [SerializeField]
    private GameObject headLinePrefab; // Prefab for the head line

    [SerializeField]
    private float headLineOffsetY = 1.5f; // Offset from player's position
    private GameObject currentHeadLine;

    [Header("Speed Settings")]
    [SerializeField]
    private float speedIncreaseInterval = 5f; // Time in seconds between speed increases
    [SerializeField]
    private float speedIncreaseAmount = 0.5f; // Amount by which speed increases

    private float currentSpeed = 2f; // Initial speed of the player
    private float speedTimer;

    private void Awake()
    {
        playing = false;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        rb = GetComponent<Rigidbody2D>();
        inputManager = InputManager.Instance;

        startingPosition = transform.position;
    }

    private void OnEnable()
    {
        InputManager.OnPressPlay += StartGame;
    }

    private void OnDisable()
    {
        InputManager.OnPressPlay -= StartGame;
    }

    private void StartGame()
    {
        playing = !playing;

        if (playing)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            cameraFollow.enabled = true;
            gameStarted = true;

            // Spawn the head line dynamically once
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            cameraFollow.enabled = false;
            gameStarted = false;

            // Destroy head line when game stops
            if (currentHeadLine != null)
            {
                Destroy(currentHeadLine);
            }

            // Stop background music
            if (backgroundMusic != null && backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
        }
    }

    private void Update()
    {
        if (playing && gameStarted)
        {
            // Start or play background music
            if (backgroundMusic != null && !backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }

            // Update the head line position dynamically to follow the player's head
            UpdateHeadLinePosition();

            // Track time and update speed every interval
            timePlaying += Time.deltaTime;
            speedTimer += Time.deltaTime;

            int score = Mathf.FloorToInt(timePlaying);
            scoreTextUI.text = "Score: " + score.ToString();

            // Increase speed every 10 seconds
            if (speedTimer >= speedIncreaseInterval)
            {
                IncreaseSpeed();
                speedTimer = 0f; // Reset the speed timer
            }

            // Apply the current speed to the player's movement
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

            CheckGameOver();
        }
    }

    private void UpdateHeadLinePosition()
    {
        if (currentHeadLine != null)
        {
            // Update the position of the head line relative to the player's position
            currentHeadLine.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + headLineOffsetY,
                0f
            );
        }
    }

    private void IncreaseSpeed()
    {
        currentSpeed += speedIncreaseAmount;
    }

    private void CheckGameOver()
    {
        float yDifference = Mathf.Abs(transform.position.y - Camera.main.transform.position.y);

        if (yDifference >= maxYDifference)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        playing = false;
        rb.bodyType = RigidbodyType2D.Static;

        // Stop background music on game over
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }

        // Destroy the current headline
        if (currentHeadLine != null)
        {
            Destroy(currentHeadLine);
        }

        string scoreText = scoreTextUI.text;
        int score = 0;

        // Assuming the scoreText is in the format "Score: 123"
        if (scoreText.StartsWith("Score: "))
        {
            string scoreSubstring = scoreText.Substring(7); // Remove "Score: " (7 characters)
            if (int.TryParse(scoreSubstring, out score))
            {
                // Store the score in PlayerPrefs
                PlayerPrefs.SetInt("PlayerScore", score);
                PlayerPrefs.Save(); // Optional: to ensure it's saved immediately
            }
            else
            {
                Debug.LogError("Failed to parse score from text!");
            }
        }
        else
        {
            Debug.LogError("Score text is not in the expected format!");
        }

        // Load the Game Over scene
        SceneManager.LoadScene(gameOverSceneName);
    }
}
