using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Burner
{
    public bool InUse;
    public Image CookingStatusBar;
    public TMP_Text CookingStatusText;
}
public class Kitchen : MonoBehaviour
{
    [SerializeField] private FoodList foodList;
    [SerializeField] private OrderList Delivery;
    [SerializeField] private Burner[] Slots= new Burner[2];
    public static Action<int> Payment;
    public static Action<Customer> Dining;
    public static  bool slotAvailable;

    private void Awake()
    {
        slotAvailable = true;
    }
    void OnEnable()
    {
        OrderItem.Availability += CheckSlotAvailability;
        OrderItem.OrderProcessing += CheckAvailabilty;
    }

    bool CheckSlotAvailability()
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            if(!Slots[i].InUse)
            {
                return true;
            }
        }
        return false;
    }
    private void CheckAvailabilty(int orderId, string itemName)
    {
        int cookingSlot = 0;
        int preparationTime=0;
        int timeToEat=0;
        for (int i = 0; i < Slots.Length; i++)
        {
            if (!Slots[i].InUse)
            {
                Slots[i].InUse = true;
                cookingSlot = i;
                break;
            }
        }
        slotAvailable = CheckSlotAvailability();

        for (int i = 0; i < foodList.AllItems.Length; i++)
        {
            if (itemName == foodList.AllItems[i].itemName)
            {
                Payment(foodList.AllItems[i].itemPrice);
                preparationTime = foodList.AllItems[i].preparationTime;
                timeToEat = foodList.AllItems[i].consumingTime;
                break;
            }
        }
        
        StartCoroutine(Cook(cookingSlot, orderId, preparationTime, timeToEat, itemName));
    }

    IEnumerator Cook(int cookingSlot,int orderId,int preparationTime,int timetoEat,string itemName)
    {
        float timeElapsed = 0;
        Slots[cookingSlot].CookingStatusText.text = itemName;
        while (preparationTime > timeElapsed)
        {
            timeElapsed += 0.1f;
            Slots[cookingSlot].CookingStatusBar.fillAmount = (timeElapsed / preparationTime);
            yield return new WaitForSeconds(0.1f);
        }
        Slots[cookingSlot].InUse = false;
        Slots[cookingSlot].CookingStatusBar.fillAmount = 1;
        Slots[cookingSlot].CookingStatusText.text = "Not Cooking";
        slotAvailable = true;
        /*StopAllCoroutines();*/
        StopCoroutine(Cook(cookingSlot, orderId,preparationTime,timetoEat,itemName));
        Deliver(orderId,timetoEat);
    }

    void Deliver(int orderId,int timeToEat)
    {
        slotAvailable = CheckSlotAvailability();
        for(int i = 0; i < Delivery.waitingCustomers.Count; i++)
        {
            if (Delivery.waitingCustomers[i].orderId == orderId)
            {
                /*yield return new WaitForSeconds(1);*/
                Customer customer = Delivery.waitingCustomers[i];
                customer.timeToConsume = timeToEat;
                Dining(customer);
                Delivery.waitingCustomers.Remove(Delivery.waitingCustomers[i]);
                break;
            }
        }
    }
    private void OnDisable()
    {
        OrderItem.Availability -= CheckSlotAvailability;
        OrderItem.OrderProcessing -= CheckAvailabilty;
    }
}
