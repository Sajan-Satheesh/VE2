using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] Image EatFilll;
    [SerializeField] int timeElapsedEating;
    public NPC npcCharacter { get; private set; }
    public GameObject npcCanvasBar;
    public int orderId { get; set; }
    public bool IsCustomer { get;  private set; }
    public float timeToConsume;
    public float fillAmount { set => EatFilll.fillAmount = value; }

    private void Awake()
    {
        IsCustomer = false;
        npcCanvasBar.SetActive(false);
        npcCharacter = GetComponent<NPC>();
    }
    private void OnEnable()
    {
        OrderItem.OrderProcessing += GetOrderId;
    }

    public void SetAsCustomer()
    {
        IsCustomer = true;
    }

    public void Leave()
    {
        ResetCustomer();
        npcCharacter.ResetCharacter();
    }

    private void ResetCustomer()
    {
        IsCustomer = false;
        npcCanvasBar.SetActive(false);
        fillAmount = 0f;
    }

    public void placeOrder()
    {
        npcCharacter.SetForwardDirection(Vector3.up);
        MenuAllItem AvailableItems = WorldManager.MenuItemsList.GetComponent<MenuAllItem>();
        int totalAvailableItems=0;
        MenuItem selectedItem;
        totalAvailableItems = AvailableItems.menuItems.Count;
        npcCanvasBar.SetActive(true);
        if (totalAvailableItems > 0)
        {
            selectedItem = AvailableItems.menuItems[Random.Range(0, totalAvailableItems)];
            WorldManager.orderList.TakeOrder(selectedItem, 1, this);
        }
        else Leave();
    }

    void GetOrderId(int orderId, string item)
    {
        this.orderId = orderId;
    }

    
    
    public void StartToEat()
    {
        StartCoroutine(Eat());
    }


    private IEnumerator Eat()
    {
        timeElapsedEating = 0;
        EatFilll.fillAmount = 0;
        while (timeToConsume > timeElapsedEating)
        { 
            timeElapsedEating+=1;
            fillAmount = (float)timeElapsedEating / timeToConsume;
            yield return new WaitForSeconds(1);
        }
        Leave();
        StopAllCoroutines();
    }

}
