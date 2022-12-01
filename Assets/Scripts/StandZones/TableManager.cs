
using UnityEngine;


public class TableManager : MonoBehaviour
{

    public bool tableOccupied;
    public Chair[] chairs = new Chair[4];
    void Awake()
    {
        tableOccupied = false;
        for (int i = 0; i < chairs.Length; i++)
        {
            chairs[i].occupied = false;
        }
        
    }

}
