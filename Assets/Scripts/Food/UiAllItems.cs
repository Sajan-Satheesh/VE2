using UnityEngine;

public class UiAllItems : MonoBehaviour
{
    public ItemUI itemUI;
    public FoodList foodList;
    int itemCount;

    void Awake()
    {
        itemCount = foodList.AllItems.Length;
    }
    private void Start()
    {
        for (int i = 0; i < itemCount; i++)
        {
            itemUI.itemImage.sprite = foodList.AllItems[i].itemImage;
            itemUI.itemName.text = foodList.AllItems[i].itemName;
            itemUI.itemPrice.text = foodList.AllItems[i].itemPrice.ToString();
            itemUI.fooditem = foodList.AllItems[i].itemVariety;
            Instantiate(itemUI, gameObject.transform);
        }
    }

}
