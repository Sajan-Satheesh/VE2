using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    TMP_Text highScoreText;

    private void Awake()
    {
        highScoreText = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        highScoreText.text = "Rs. " + GameManager.GameManagerInstance.HighScore.ToString();
    }
}
