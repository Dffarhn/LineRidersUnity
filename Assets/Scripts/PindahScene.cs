using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahScene : MonoBehaviour
{
    
    public void pindahScene(string tujuan)
    {
        Debug.Log("Attempting to load scene: " + tujuan);
        SceneManager.LoadScene(tujuan);
    }

    public void TestButton()
    {
        Debug.Log("Button clicked!");

        SceneManager.LoadScene("GamePlay");
    }
}
