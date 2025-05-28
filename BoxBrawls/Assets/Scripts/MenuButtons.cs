using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public Button easy;
    public Button medium;
    public Button hard;

    public Button quit;

    // Load the "player type" scene when the play button is clicked.
    public void playeasy()
    {
        GameSettings.enemyHealth = 100f;
        SceneManager.LoadScene("Fight");
    }

    public void playmedium()
    {
        GameSettings.enemyHealth = 200f;
        SceneManager.LoadScene("Fight");
    }

    public void playhard()
    {
        GameSettings.enemyHealth = 400f;
        SceneManager.LoadScene("Fight");
    }


    // Quit the application when the quit button is clicked.
    public void quitgame()
    {
        Application.Quit();
    }

}
