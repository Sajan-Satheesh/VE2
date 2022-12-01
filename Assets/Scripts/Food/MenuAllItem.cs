
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAllItem : MonoBehaviour
{
    public MenuItem menuItem;
    public ItemUI SourceItem;
    public List<MenuItem> menuItems = new List<MenuItem>();

    private void Awake()
    {
        SourceItem = null;
    }
    public void InstantiateMenuItem(ItemUI Item)
    {
        SourceItem = Item;
        menuItem.itemImage.sprite = SourceItem.itemImage.sprite;
        menuItem.itemName.text = SourceItem.itemName.text;
        menuItem.itemPrice.text = SourceItem.itemPrice.text;
        menuItem.fooditem = SourceItem.fooditem;
        menuItems.Add(Instantiate(menuItem, gameObject.transform));
    }
}
