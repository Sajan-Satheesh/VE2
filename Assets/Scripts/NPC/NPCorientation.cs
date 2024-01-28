
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

    protected Vector2 SpawnRandomInBounds(Vector2 min, Vector2 max)
    {
        Vector2 position = new Vector2(0, 0);
        float yPos = Random.Range(min.y, max.y);
        int side = Random.Range(0,2);
        position = (side) switch
        {
            0 => new Vector2(min.x, yPos),
            1 => new Vector2(max.x, yPos),
            _ => new Vector2(max.x, yPos),
        };
        return position;
    }

    protected Direction GetNpcDirection(Vector2 location)
    {
        if(location.x >= MaxBound.x)
        {
            return Direction.right;
        }
        if (location.x <= MinBound.x)
        {
            return Direction.left;
        }
        return default;
    }

}
