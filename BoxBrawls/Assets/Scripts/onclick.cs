using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class onclick : MonoBehaviour
{
    // This script handles button clicks for restarting the game and going to the main menu.
    // It uses Unity's SceneManager to load different scenes based on button clicks.
   public Button button;

   public Button quit;
   
 public void restartgame()
    {
        // Reset slowmo before reloading the scene
        if (SlowmoManager.Instance != null)
        {
            SlowmoManager.Instance.ResetSlowmo();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainmenu()
    {
        // Reset slowmo before going to main menu
        if (SlowmoManager.Instance != null)
        {
            SlowmoManager.Instance.ResetSlowmo();
        }
        SceneManager.LoadScene("MainMenu");
    }
}

