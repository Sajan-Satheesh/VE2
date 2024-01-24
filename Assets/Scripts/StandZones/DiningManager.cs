using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningManager : MonoBehaviour
{

    [SerializeField]TableManager[] Tables = new TableManager[4];

    private void OnEnable()
    {
        Kitchen.Dining += CheckSeatAvailability;
    }

    void CheckSeatAvailability(Customer customer)
    {
        NPCMovement PurchasedCustomer = customer.GetComponent<NPCMovement>();
        for(int i=0; i<Tables.Length; i++)
        {
            if (!Tables[i].tableOccupied)
            {
                bool once = false;
                for(int j=0; j < Tables[i].chairs.Length; j++)
                {
                    if (!Tables[i].chairs[j].occupied)
                    {
                        if (once)
                        {
                            break;
                        }
                        PurchasedCustomer.SeatTarget = Tables[i].chairs[j].transform;
                        PurchasedCustomer.StandzoneTarget = null;
                        Tables[i].chairs[j].occupied = true;
                        Tables[i].chairs[j].chairAllotedCustomer = customer;
                        once = true;
                    }
                }
                if (once)
                {
                    break;
                }
                Tables[i].tableOccupied = true;
                break;
            }
            else
            {
                customer.gameObject.GetComponent<NPCMovement>().SeatTarget = null;
            }
        }
    }

    private void OnDisable()
    {
        Kitchen.Dining -= CheckSeatAvailability;
    }

}
