using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField; // Reference to the TextMeshPro Input Field
    public TextMeshProUGUI playerNameText; // Reference to the TextMeshPro text above the player
    public GameObject startButton; // Reference to the Start Button GameObject

    [Header("Player Reference")]
    public GameObject player; // Reference to the Player GameObject

    private void Start()
    {
        // Clear any text at the start
        playerNameText.text = "";
        player.SetActive(false); // Ensure the player is initially inactive
    }

    public void SubmitName()
    {
        if (!string.IsNullOrEmpty(nameInputField.text))
        {
            // Set the name text above the player
            playerNameText.text = nameInputField.text;

            // Hide the input field and the start button
            nameInputField.gameObject.SetActive(false);
            startButton.SetActive(false);

            // Activate the player or start the game
            player.SetActive(true);
        }
    }
}
