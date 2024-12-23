using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    public float upsideDownThreshold = 175f; // Threshold for upside-down rotation (in degrees)
    public float countdownTime = 1f; // Time to trigger game over

    private float countdownTimer = 0f;
    private bool isCountingDown = false;
    private CircleCollider2D headCollider;

     [SerializeField]
    private TextMeshProUGUI scoreTextUI;

    private void Start()
    {
        // Get the Circle Collider 2D component from the player
        headCollider = GetComponent<CircleCollider2D>();
        if (headCollider == null)
        {
            Debug.LogError("CircleCollider2D not found on the player! Please add one.");
        }
        else
        {
            Debug.Log("CircleCollider2D found on the player.");
        }
    }

    private void Update()
    {
        if (headCollider == null)
            return;

        // Check if the player is upside down
        if (IsPlayerUpsideDown())
        {
            Debug.Log("Player is upside down.");
            
            // Check for collisions with lines on the "Lines" layer
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                headCollider.bounds.center,
                headCollider.radius,
                1 << 3 // Layer 3 corresponds to "Lines"
            );

            if (hits.Length > 0)
            {
                // Start or continue the countdown
                if (!isCountingDown)
                {
                    Debug.Log("Collision detected, starting countdown.");
                }
                
                isCountingDown = true;
                countdownTimer += Time.deltaTime;
                Debug.Log($"Countdown Timer: {countdownTimer}s");

                if (countdownTimer >= countdownTime)
                {
                    GameOver();
                }
            }
            else
            {
                // Reset the countdown if no collision is detected
                if (isCountingDown)
                {
                    Debug.Log("No collision detected, resetting countdown.");
                }
                ResetCountdown();
            }
        }
        else
        {
            // Reset the countdown if the player is not upside down
            if (isCountingDown)
            {
                Debug.Log("Player is not upside down, resetting countdown.");
            }
            ResetCountdown();
        }
    }

    private bool IsPlayerUpsideDown()
    {
        float rotationZ = transform.eulerAngles.z;
        Debug.Log($"Player rotation Z: {rotationZ}");
        return Mathf.Abs(rotationZ - 180f) < upsideDownThreshold;
    }

    private void ResetCountdown()
    {
        isCountingDown = false;
        countdownTimer = 0f;
        Debug.Log("Countdown reset.");
    }

    private void GameOver()
    {
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
        SceneManager.LoadScene("GameOverScene");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the Circle Collider's area in the editor for debugging
        if (headCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(headCollider.bounds.center, headCollider.radius);
            Debug.Log("Gizmos: Drawing Circle Collider area.");
        }
    }
}
