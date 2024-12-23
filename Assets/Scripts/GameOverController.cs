using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        // Retrieve the score from PlayerPrefs
        int score = PlayerPrefs.GetInt("PlayerScore", 0); // Default value of 0 if the score doesn't exist

        // Display the score
        scoreText.text = score.ToString();
    }
}
