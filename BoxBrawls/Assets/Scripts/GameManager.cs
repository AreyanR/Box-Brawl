using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [Header("Game Over UI")]
    public TextMeshProUGUI gameOverText;
    public GameObject playAgainButton;
    public GameObject quitGameButton;

    private bool isGameOver = false;

    void Awake()
    {
        // Setup singleton
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
    }

    public void PlayerDied()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            ShowGameOverUI("You Died!");
        }
    }

    public void EnemyDied()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            ShowGameOverUI("You Win!");
        }
    }

    private void ShowGameOverUI(string message)
    {
        gameOverText.text = message;
        gameOverText.enabled = true;
        playAgainButton.SetActive(true);
        quitGameButton.SetActive(true);
    }

    private void HideGameOverUI()
    {
        gameOverText.enabled = false;
        playAgainButton.SetActive(false);
        quitGameButton.SetActive(false);
    }
}
