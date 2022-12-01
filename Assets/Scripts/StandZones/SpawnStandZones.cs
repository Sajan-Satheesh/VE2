using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnStandZones : MonoBehaviour
{
    [SerializeField] int zoneNumbers;
    public int WaitCount { get => zoneNumbers;  }
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
            Zones.Add(Instantiate(Zone,transform));
            yield return new WaitForSeconds(0.1f);
        }
        StopAllCoroutines();
    }
}