using UnityEngine;

public class HastleFreeArea : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Customer customer))
        {
            customer.GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Customer customer))
        {
            customer.GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }

}
