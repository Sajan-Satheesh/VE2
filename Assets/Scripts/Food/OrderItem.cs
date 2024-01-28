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

    //static public Action<Customer> CancelOrder { get; set; }
    static public Func<bool> CheckStoves;
    static public Action <int,string>OrderProcessing;
    private bool accepted;

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
            customer.Leave();
        }
    }

    private void SendtoKitchen()
    {
        if ((bool)CheckStoves?.Invoke())
        {
            accepted = true;
            gameObject.GetComponentInParent<OrderList>().waitingCustomers.Add(customer);
            OrderProcessing(++orderId, itemName.text);
            RemoveItem();
        }
    }
}
