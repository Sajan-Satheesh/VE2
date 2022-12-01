using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Transform StallPosition;
    [SerializeField] Transform TablePosition;
    [SerializeField] SpawnStandZones spawnStandZones;
    [SerializeField] CrowdSimulator NpcObjects;
    [SerializeField] private RectTransform MapObject;
    [SerializeField] GameObject PauseScreen;
    private int zoneCount;
    static public bool pause;
    static public Transform stallPosition;
    static public Bounds mapBoundary;
    static public List<NPCmovement> CustomerNpc;
    static public Vector2 tablePosition;
    public MenuAllItem menuItemsList;
    public UiAllItems allItemsList;
    public OrderList OrderList;
    static public MenuAllItem MenuItemsList;
    static public UiAllItems AllItemsList;
    static public OrderList orderList;

    void Awake()
    {
        MenuItemsList = menuItemsList;
        AllItemsList = allItemsList;
        orderList = OrderList;
        tablePosition = TablePosition.position;
        stallPosition = StallPosition;
        pause = false;
        CustomerNpc = new List<NPCmovement>();
        mapBoundary.size = MapObject.sizeDelta;
    }
    private void OnEnable()
    {
        StandZone.AddCustomer += DrawCustomer;
    }
    private void Start()
    {
        zoneCount = spawnStandZones.WaitCount;
        StartCoroutine(SetupCustomers());
    }

    IEnumerator SetupCustomers()
    {
        yield return new WaitForSeconds(5);
        int nextCustomerArrival;
        for (int i = 0; i < zoneCount; i++)
        {
            nextCustomerArrival = Random.Range(5, 25);
            yield return new WaitForSeconds(nextCustomerArrival);
            StandZone standZone = spawnStandZones.Zones[i].GetComponent<StandZone>();
            DrawCustomer(standZone);
        }
        StopAllCoroutines();
    }

    private void DrawCustomer(StandZone standZone)
    {
        int index;
        Debug.Log("Started Next Search");
        int availableNpcCount = NpcObjects.npcObjects.Count;
        NPCmovement selectedNpc;
        while (standZone.AllotedObject == null)
        {
            Debug.Log("Started Checking");
            index = Random.Range(0, availableNpcCount);
            selectedNpc = NpcObjects.npcObjects[index].GetComponent<NPCmovement>();
            if (!selectedNpc.IsCustomer)
            {
                standZone.AllotedObject = selectedNpc.gameObject;
                selectedNpc.IsCustomer = true;
                selectedNpc.StandzoneTarget = standZone.transform;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
            {
                Time.timeScale = 0;
                pause = true;
                PauseScreen.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pause = false;
                PauseScreen.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        StandZone.AddCustomer -= DrawCustomer;
    }
}
