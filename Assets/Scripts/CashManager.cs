
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    int TotalCash;
    public int totalCash { get=>TotalCash; }
    TMP_Text UiCashText;
    private void Awake()
    {
        UiCashText = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        Kitchen.Payment += CollectCash;
    }

    private void CollectCash(int cash)
    {
        TotalCash += cash;
        UiCashText.text = "Total Cash: "+ TotalCash.ToString();
    }

    private void OnDisable()
    {
        Kitchen.Payment -= CollectCash;
    }
}
