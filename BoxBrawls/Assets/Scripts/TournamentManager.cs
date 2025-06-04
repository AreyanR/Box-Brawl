using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;


public class TournamentManager : MonoBehaviour
{

    public static TournamentManager Instance;
    public int currentStage = 0;
    public int totalStages = 3;
    public float[] enemyHealthPerStage;


    private void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartTournament()
    {
        currentStage = 0;
        LoadNextFight();
    }

    public void LoadNextFight()
    {
        if (currentStage >= totalStages)
        {
            //SceneManager.LoadScene("TournamentVictory");
            return;
        }

        GameSettings.enemyHealth = enemyHealthPerStage[currentStage];
        currentStage++;
        SceneManager.LoadScene("Fight");
    }

    public void OnPlayerVictory()
    {
        if (currentStage >= totalStages)
        {
            GameManager.Instance.ShowGameOverUI("You Win!");
            return;
        }

        GameManager.Instance.ShowGameOverUI("Stage " + currentStage + "/" + totalStages + " Complete!");
        GameManager.Instance.HideGameOverButtons();
        StartCoroutine(DelayedNextFight());
    }

    public void OnPlayerDefeat()
    {
        SceneManager.LoadScene("TournamentLose");
    }

    private IEnumerator DelayedNextFight()
    {
        yield return new WaitForSeconds(5f);
        LoadNextFight();
    }
}
