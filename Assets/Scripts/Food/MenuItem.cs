using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public Button delete;
    public Image itemImage;
    public Text itemName;
    public Text itemPrice;
    public FooditemTypes fooditem;
    // Start is called before the first frame update

    private void Start()
    {
        delete.onClick.AddListener(RemoveItem);
    }

    private void RemoveItem()
    {
        int itemNums = WorldManager.AllItemsList.transform.childCount;
        for(int i=0; i<itemNums; i++)
        {
            if(fooditem == WorldManager.AllItemsList.transform.GetChild(i).GetComponent<ItemUI>().fooditem)
            {
                Debug.Log("entered");
                WorldManager.AllItemsList.transform.GetChild(i).GetComponent<ItemUI>().AddedWatermark.SetActive(false);
            }
        }
        WorldManager.MenuItemsList.GetComponent<MenuAllItem>().menuItems.Remove(gameObject.GetComponent<MenuItem>());
        Destroy(gameObject);
    }
}
