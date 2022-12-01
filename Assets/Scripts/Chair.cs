
using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] int chairNumber;
    public bool occupied;
    public Customer chairAllotedCustomer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (chairAllotedCustomer!=null && chairAllotedCustomer.name == collision.name)
        {
            Debug.Log("Seated");
            chairAllotedCustomer.restState = true;
            chairAllotedCustomer.StartToEat(chairNumber);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (chairAllotedCustomer != null && chairAllotedCustomer.name == collision.name)
        {
            chairAllotedCustomer = null;
            occupied = false;
            GetComponentInParent<TableManager>().tableOccupied = false;
        }
    }
}
