
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button Resume;
    [SerializeField] private Button MainMenu;

    private void Awake()
    {
        Resume.onClick.AddListener(ResumeGame);
        MainMenu.onClick.AddListener(QuitToMainMenu);
    }

    private void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
