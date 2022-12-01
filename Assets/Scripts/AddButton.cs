using UnityEngine;
using UnityEngine.UI;

public class AddButton : MonoBehaviour
{
    public GameObject ObjectToEnable;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(EnableObjects);
    }

    private void EnableObjects()
    {
        ObjectToEnable.SetActive(true);
    }
}
