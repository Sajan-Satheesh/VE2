
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Button Replay;
    [SerializeField] private Button MainMenu;
    [SerializeField] private TMP_Text Message;
    [SerializeField] private TMP_Text DisplayScore;
    [SerializeField] private TMP_Text HighScore;
    [SerializeField] private CashManager scoring;

    private void Awake()
    {
        Replay.onClick.AddListener(PlayAgainGame);
        MainMenu.onClick.AddListener(QuitToMainMenu);
    }
    private void OnEnable()
    {
        Time.timeScale = 0;
        DisplayScore.text = scoring.totalCash.ToString();
        if (scoring.totalCash > GameManager.GameManagerInstance.HighScore)
        {
            GameManager.GameManagerInstance.HighScore = scoring.totalCash;
            Message.text = "Congratulations!! You have set a new high score";
        }
        else Message.text = "Your total earnings is";
        HighScore.text = GameManager.GameManagerInstance.HighScore.ToString();
    }

    private void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void PlayAgainGame()
    {
        SceneManager.LoadScene(1);
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
