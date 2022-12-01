
using UnityEngine;

public enum Direction
{
    up,
    down,
    left,
    right
}
public class NPCorientation : MonoBehaviour
{
    [SerializeField] protected Bounds MapBoundary;

    protected Vector2 MinBound { get => MapBoundary.min; }
    protected Vector2 MaxBound { get => MapBoundary.max; }

    private void Start()
    {
        MapBoundary.size = WorldManager.mapBoundary.size;
    }

    protected Vector2 InstantiateLocation(Vector2 min, Vector2 max)
    {
        Vector2 position = new Vector2(0, 0);
        float yPos = Random.Range(min.y, max.y);
        int side = Random.Range(0,2);
        switch (side)
        {
            case 0: position = new Vector2(min.x, yPos);  break;
            case 1: position = new Vector2(max.x, yPos);  break;
        }
        return position;
    }

    protected void NPCdirection(Vector2 location, NPCmovement npc)
    {
        if(location.x > MaxBound.x)
        {
            npc.npcStartDirection = Direction.right;
        }
        if (location.x < MinBound.x)
        {
            npc.npcStartDirection = Direction.left;
        }
    }

}
