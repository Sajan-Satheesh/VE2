
using UnityEngine;


public class TableManager : MonoBehaviour
{

    public bool tableOccupied;
    public Chair[] chairs;
    void Awake()
    {
        tableOccupied = false;
        for (int i = 0; i < chairs.Length; i++)
        {
            chairs[i].occupied = false;
        }
        
    }

}
