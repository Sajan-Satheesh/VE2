
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool occupied;
    public Customer chairAllotedCustomer { get; set; }

    public void ResetChair()
    {
        chairAllotedCustomer = null;
        occupied = false;
        GetComponentInParent<TableManager>().tableOccupied = false;
    }
}
