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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainmenu()
    {
        //SceneManager.LoadScene("Main Menu"); for future development
    }

}
