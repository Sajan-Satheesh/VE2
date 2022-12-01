using System;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : MonoBehaviour
{
    
    public Image itemImage;
    public Text itemName;
    public Text quantity;
    public Button delete;
    public Button accept;
    public MenuItem menuItem;
    public Customer customer;
    public int orderId;

    static public Action<Customer> CancelOrder;
    static public Func<bool> Availability;
    static public Action <int,string>OrderProcessing;
    private bool accepted;
    private bool slotsAvailable;

    private void Awake()
    {
        accepted = false;
        orderId = 0;
    }
    void Start()
    {
        delete.onClick.AddListener(RemoveItem);
        accept.onClick.AddListener(SendtoKitchen);
    }

    private void RemoveItem()
    {
        Destroy(gameObject);
        gameObject.GetComponentInParent<OrderList>().orderedItems.Remove(this);
        if (!accepted)
        {
            CancelOrder(customer);
        }
    }

    private void SendtoKitchen()
    {
        slotsAvailable = Availability();
        Debug.Log(slotsAvailable);
        if (slotsAvailable)
        {
            accepted = true;
            gameObject.GetComponentInParent<OrderList>().waitingCustomers.Add(customer);
            OrderProcessing(orderId, itemName.text);
            RemoveItem();
        }
    }
}
