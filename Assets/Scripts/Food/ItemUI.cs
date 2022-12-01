using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemImage;
    public Text itemName;
    public Text itemPrice;
    public FooditemTypes fooditem;
    public GameObject AddedWatermark;
    MenuAllItem MenuItems;

    private void Start()
    {
        MenuItems = WorldManager.MenuItemsList.GetComponent<MenuAllItem>();
        gameObject.GetComponent<Button>().onClick.AddListener(AddToMenu);
    }
    void AddToMenu()
    {
        if (!AddedWatermark.activeSelf)
        {
            AddedWatermark.SetActive(true);
            MenuItems.InstantiateMenuItem(this);
        }
        
    }
}
