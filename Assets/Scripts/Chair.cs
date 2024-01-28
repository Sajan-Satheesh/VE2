
using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] int chairNumber;
    public bool occupied;
    public Customer chairAllotedCustomer { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (chairAllotedCustomer!=null && chairAllotedCustomer.gameObject.name == collision.gameObject.name)
        {
            Debug.Log("Seated");
            chairAllotedCustomer.restState = true;
            chairAllotedCustomer.StartToEat(chairNumber);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (chairAllotedCustomer != null && chairAllotedCustomer.gameObject.name == collision.gameObject.name)
        {
            chairAllotedCustomer = null;
            occupied = false;
            GetComponentInParent<TableManager>().tableOccupied = false;
        }
    }
}
