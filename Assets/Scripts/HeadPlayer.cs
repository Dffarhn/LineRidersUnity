using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Lines"))
        {
            Debug.Log("Headline hit a line! Game Over.");
            GameOver();
        }
    }

    private void GameOver(){
    


        // Load the Game Over scene
        SceneManager.LoadScene("GameOver");
    }
  
}
