
using UnityEngine;
using UnityEngine.UI;

public class OenWindow : MonoBehaviour
{
    Button button;
    [SerializeField] GameObject PopUpPanel;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    void Start()
    {
        button.onClick.AddListener(ObjectToOpen);
    }

    private void ObjectToOpen()
    {
        PopUpPanel.SetActive(true);
    }
}
