
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeSliderNum : MonoBehaviour
{
    [SerializeField] Slider GameTimeSelector;
    [SerializeField] TMP_Text GameTime;

    private void Update()
    {
        GameTime.text = (2*(int)GameTimeSelector.value).ToString();
    }
}
