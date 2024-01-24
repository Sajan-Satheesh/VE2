using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnStandZones : MonoBehaviour
{
    [field : SerializeField] public int zoneNumbers { get; private set; }
    [SerializeField] GameObject Zone;
    public List<GameObject> Zones;

    private void Start()
    {
        StartCoroutine(InitializeZones(zoneNumbers)) ;
    }
    IEnumerator InitializeZones(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 position = new(transform.position.x + Random.Range(-1f, 1f), transform.position.y - 3f, transform.position.z);
            Zones.Add(Instantiate(Zone,position,Quaternion.identity,transform));
            yield return new WaitForSeconds(1f);
        }
        StopAllCoroutines();
    }
}