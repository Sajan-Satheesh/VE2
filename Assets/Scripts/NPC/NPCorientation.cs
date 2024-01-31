
using UnityEngine;

public class NPCorientation : MonoBehaviour
{
    [SerializeField] protected Bounds MapBoundary;

    protected Vector2 MinBound { get => MapBoundary.min; }
    protected Vector2 MaxBound { get => MapBoundary.max; }

    private void Start()
    {
        MapBoundary = WorldManager.mapBoundary;
    }

    protected Vector2 SpawnRandomInBounds()
    {
        Vector2 position = new Vector2(0, 0);
        float yPos = Random.Range(MinBound.y, MaxBound.y);
        int side = Random.Range(0,2);
        position = (side) switch
        {
            0 => new Vector2(MinBound.x, yPos),
            1 => new Vector2(MaxBound.x, yPos),
            _ => new Vector2(MaxBound.x, yPos),
        };
        return position;
    }

    protected Vector3 GetSpawnNpcDirection(Vector2 location)
    {
        if(location.x >= MaxBound.x)
        {
            return Vector3.left;
        }
        if (location.x <= MinBound.x)
        {
            return Vector3.right;
        }
        return default;
    }

}
