using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //this script manages the game state, including player and enemy deaths, and displays the Game Over UI.
    public static GameManager Instance;

    // UI elements for the Game Over screen
    [Header("Game Over UI")]
    public TextMeshProUGUI gameOverText;
    [Header("Round Info UI")]
    public TextMeshProUGUI roundText;
    public GameObject playAgainButton;
    public GameObject quitGameButton;

    private bool isGameOver = false;

    void Awake()
    {
        // Make sure the Game Over UI is hidden at the start
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        HideGameOverUI();
        HideGameOverButtons();
    }

    public void PlayerDied()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            ShowGameOverUI("You Died!");
            ShowGameOverButtons();
        }
    }

    public void EnemyDied()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            ShowGameOverUI("You Win!");
            ShowGameOverButtons();
        }
        TournamentManager.Instance.OnPlayerVictory();
    }

    public void ShowGameOverButtons()
    {
        playAgainButton.SetActive(true);
        quitGameButton.SetActive(true);
    }
    public void ShowGameOverUI(string message)
    {
        gameOverText.text = message;
        gameOverText.enabled = true;
    }

    public void HideGameOverUI()
    {
        gameOverText.enabled = false;     
    }
    public void HideGameOverButtons()
    {
        playAgainButton.SetActive(false);
        quitGameButton.SetActive(false);
    }
}
