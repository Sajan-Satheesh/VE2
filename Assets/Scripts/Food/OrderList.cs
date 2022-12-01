using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class OrderList : MonoBehaviour
{
    public OrderItem orderItem;
    public List<OrderItem> orderedItems = new List<OrderItem>();
    public List<Customer> waitingCustomers = new List<Customer>();
    int orderId;

    private void Awake()
    {
        orderId = 0;
    }
    public void TakeOrder(MenuItem selectedMenuItem, int quantity, Customer customer)
    {
        orderId++;
        orderItem.itemName.text = selectedMenuItem.itemName.text;
        orderItem.itemImage.sprite = selectedMenuItem.itemImage.sprite;
        orderItem.quantity.text = quantity.ToString();
        orderItem.customer = customer;
        orderItem.orderId = orderId;
        orderedItems.Add(Instantiate(orderItem, gameObject.transform));
    }
}
