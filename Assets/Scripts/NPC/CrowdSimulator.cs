using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSimulator : NPCorientation
{
    [SerializeField] List <Customer> customerNPCs;
    [SerializeField] GameObject NPC;
    [SerializeField] int NPCCount;
    [SerializeField] public List<GameObject> npcObjects = new List<GameObject>();
    protected GameObject Obj;
    [SerializeField] Vector2 minBound;
    [SerializeField] Vector2 maxBound;

    void Start()
    {
        MapBoundary.size = WorldManager.mapBoundary.size;
        minBound = MinBound;
        maxBound = MaxBound;
        StartCoroutine(SpawnNPC(NPC, NPCCount));

    }
    protected IEnumerator SpawnNPC(GameObject Object, int count)
    {
        for (int i = 0; i < count; i++)
        {
            
            npcObjects.Add(Instantiate(Object, transform));
            
            Obj=npcObjects[i];
            Obj.transform.position = InstantiateLocation(minBound, maxBound);
            NPCdirection(Obj.transform.position, Obj.GetComponent<NPCmovement>());
            Obj.transform.position = InstantiateLocation(MinBound, MaxBound);
            Obj.name = "npc " + i.ToString();
            WorldManager.CustomerNpc.Add(Obj.GetComponent<NPCmovement>());
            yield return new WaitForSeconds(2);

        }
    }


}
