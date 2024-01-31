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
        NPC PurchasedCustomer = customer.GetComponent<NPC>();
        bool isTableReserved = false;
        for (int i=0; i<Tables.Length; i++)
        {
            if (!Tables[i].tableOccupied)
            {
                for(int j=0; j < Tables[i].chairs.Length; j++)
                {
                    if (!Tables[i].chairs[j].occupied)
                    {
                        PurchasedCustomer.SeatTarget = Tables[i].chairs[j].transform;
                        Tables[i].chairs[j].chairAllotedCustomer = customer;
                        Tables[i].chairs[j].occupied = true;
                        isTableReserved = true;
                        PurchasedCustomer.SetState(NpcState.MOVE_TO_TABLE);
                        break;
                    }
                }
                if (isTableReserved)
                {
                    break;
                }
                Tables[i].tableOccupied = true;
            }
        }
        if (!isTableReserved)
        {
            customer.Leave();
        }
    }

    private void OnDisable()
    {
        Kitchen.Dining -= CheckSeatAvailability;
    }

}
