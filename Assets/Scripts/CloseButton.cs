using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public GameObject ObjectToDisable;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(DisableObject);
    }

    private void DisableObject()
    {
        ObjectToDisable.SetActive(false);
    }
}
