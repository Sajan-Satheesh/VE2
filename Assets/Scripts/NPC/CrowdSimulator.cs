using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSimulator : NPCorientation
{
    [SerializeField] List <Customer> customerNPCs;
    [SerializeField] NPC npc;
    [SerializeField] int NPCCount;
    [SerializeField] public List<NPC> npcObjects = new List<NPC>();
    [SerializeField] Vector2 minBound;
    [SerializeField] Vector2 maxBound;

    void Start()
    {
        MapBoundary = WorldManager.mapBoundary;
        minBound = MinBound;
        maxBound = MaxBound;
        StartCoroutine(SpawnNPC(npc, NPCCount));

    }
    protected IEnumerator SpawnNPC(NPC npcPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            npcObjects.Add(Instantiate(npcPrefab, transform));
            npcObjects[i].transform.position = SpawnRandomInBounds(minBound, maxBound);
            npcObjects[i].npcDirection = GetNpcDirection(npcObjects[i].transform.position);
            npcObjects[i].name = "npc " + i.ToString();
            WorldManager.CustomerNpc.Add(npcObjects[i]);
            yield return new WaitForSeconds(2);

        }
    }


}
