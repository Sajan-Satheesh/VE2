using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class StandZone : MonoBehaviour
{
    [SerializeField] Transform VendorPosition;
    public GameObject AllotedObject;
    static public Action PlaceOrder;
    static public Action <StandZone>AddCustomer;

    private void Awake()
    {
        AllotedObject = null;
    }

    private void Start()
    {
        VendorPosition = WorldManager.stallPosition ;
    }

    void Update()
    {
        ZoneMovement();
    }

    void ZoneMovement()
    {
        if (Vector3.Distance(VendorPosition.position, transform.position) > 1)
        {
            transform.up = (VendorPosition.position - transform.position).normalized;
            //transform.LookAt(VendorPosition, transform.localPosition);
            transform.Translate(Vector3.up * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out NPC npc))
        {
            if (AllotedObject != null && npc.name == AllotedObject.name)
            {
                npc.SetRandomDirection();
                AllotedObject = null;
                StartCoroutine(DrawNewCustomer());
            }
        }
    }

    public IEnumerator DrawNewCustomer()
    {
        int randomTime = UnityEngine.Random.Range(5, 25);
        yield return new WaitForSeconds(randomTime);
        if (AllotedObject == null)
        {
            AddCustomer(gameObject.GetComponent<StandZone>());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
