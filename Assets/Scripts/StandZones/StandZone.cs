using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class StandZone : MonoBehaviour
{
    [SerializeField] Transform VendorPosition;
    public GameObject AllotedObject;
    public Coroutine drawCustomers = null;
    static public Action PlaceOrder;
    static public Action <StandZone>AddCustomer;

    private void Awake()
    {
        AllotedObject = null;
    }

    private void Start()
    {
        VendorPosition = WorldManager.stallTransform ;
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
            transform.Translate(transform.up * Time.deltaTime);
        }
    }

    public void DrawNewCustomer()
    {
        if (drawCustomers != null)
        {
            StopCoroutine(drawCustomers);
            drawCustomers = null;
        }
        drawCustomers = StartCoroutine(DrawNew());
    }


    private IEnumerator DrawNew()
    {
        int randomTime = UnityEngine.Random.Range(5, 10);
        yield return new WaitForSeconds(randomTime);
        if (AllotedObject == null)
        {
            AddCustomer(this);
        }
    }

    public void ResetStandZone()
    {
        AllotedObject = null;
        DrawNewCustomer();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
