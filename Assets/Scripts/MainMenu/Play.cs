using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    Button button;
    [SerializeField] Slider GameTimeSelector;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    void Start()
    {
        button.onClick.AddListener(PlaySetup);
    }

    private void PlaySetup()
    {
        GameManager.GameManagerInstance.MaxGameTime = 2* (int)GameTimeSelector.value;
        SceneManager.LoadScene(1);
    }

}
