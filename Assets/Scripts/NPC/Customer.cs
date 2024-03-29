using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] Image EatFilll;
    [SerializeField] int timeElapsedEating;
    private NPCMovement npcCharacter;
    public GameObject npcCanvasBar;
    public int orderId;
    public float timeToConsume;
    public float fillAmount { set => EatFilll.fillAmount = value; }
    public Direction customerDirection { set => npcCharacter.npcStartDirection = value; }
    public bool restState { set => npcCharacter.resting = value; }

    private void Awake()
    {
        npcCanvasBar.SetActive(false);
        npcCharacter = GetComponent<NPCMovement>();
    }
    private void OnEnable()
    {
        OrderItem.OrderProcessing += GetOrderId;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out StandZone targetZone))
        {
            if (targetZone.AllotedObject != null && targetZone.AllotedObject.name == gameObject.name)
            {
                npcCharacter.resting = true;
                placeOrder();
            }
        }
    }

    public void placeOrder()
    {
        customerDirection = Direction.up;
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
        else OrderItem.CancelOrder(this);
    }

    void GetOrderId(int orderId, string item)
    {
        this.orderId = orderId;
    }

    
    
    public void StartToEat(int num)
    {
        StartCoroutine(Eat(num));
    }

    public void DiningFacingDirection(int chairNumber)
    {
        if (chairNumber == 1 || chairNumber == 2)
        {
            customerDirection = Direction.down;
        }
        else customerDirection = Direction.up;
    }
    private IEnumerator Eat(int chairNumber)
    {
        DiningFacingDirection(chairNumber);
        timeElapsedEating = 0;
        EatFilll.fillAmount = 0;
        while (timeToConsume > timeElapsedEating)
        { 
            timeElapsedEating+=1;
            fillAmount = (float)timeElapsedEating / timeToConsume;
            yield return new WaitForSeconds(1);
        }
        npcCharacter.SetDirection();
        OrderItem.CancelOrder(this);
        StopAllCoroutines();
    }

}
