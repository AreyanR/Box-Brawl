using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public Button play;
    public Button quit;

    // Load the "player type" scene when the play button is clicked.
    public void playgame()
    {
        SceneManager.LoadScene("Fight");
    }

    // Quit the application when the quit button is clicked.
    public void quitgame()
    {
        Application.Quit();
    }

}
