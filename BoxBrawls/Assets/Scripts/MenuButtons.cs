using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public Button easy;
    public Button medium;
    public Button hard;
    public Button tournament;

    public Button quit;

    // Load the "player type" scene when the play button is clicked.

    void EnsureTournamentManager()
    {
        if (TournamentManager.Instance == null)
        {
            GameObject tm = new GameObject("TournamentManager");
            tm.AddComponent<TournamentManager>();
        }
    }

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

    public void playTournament()
    {
        EnsureTournamentManager();
        TournamentManager.Instance.enemyHealthPerStage = new float[] { 10f, 20f, 40f };
        TournamentManager.Instance.totalStages = 3;
        TournamentManager.Instance.StartTournament();
    }

    // Quit the application when the quit button is clicked.
    public void quitgame()
    {
        Application.Quit();
    }

}
