using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldManager : MonoBehaviour
{
    [field :SerializeField] public Transform StallPosition { get; private set; }
    [SerializeField] Transform TablePosition;
    [SerializeField] SpawnStandZones spawnStandZones;
    [SerializeField] CrowdSimulator NpcObjects;
    [SerializeField] private RectTransform MapObject;
    [SerializeField] GameObject PauseScreen;
    private int zoneCount;
    static public bool pause;
    static public Transform stallTransform;
    static public Bounds mapBoundary;
    static public List<NPC> CustomerNpc;
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
        stallTransform = StallPosition;
        pause = false;
        CustomerNpc = new List<NPC>();
        mapBoundary.size = MapObject.sizeDelta*2;
    }
    private void OnEnable()
    {
        StandZone.AddCustomer += DrawCustomer;
    }
    private void Start()
    {
        zoneCount = spawnStandZones.zoneNumbers;
        StartCoroutine(SetupCustomers());
    }

    IEnumerator SetupCustomers()
    {
        yield return new WaitForSeconds(5);
        int nextCustomerArrival;
        for (int i = 0; i < zoneCount; i++)
        {
            nextCustomerArrival = Random.Range(2,5);
            yield return new WaitForSeconds(nextCustomerArrival);
            StandZone standZone = spawnStandZones.Zones[i].GetComponent<StandZone>();
            DrawCustomer( standZone);
        }
        StopAllCoroutines();
    }

    private void DrawCustomer( StandZone standZone)
    {
        int customerDrawIndex;
        //Debug.Log("Started Next Search");
        int availableNpcCount = NpcObjects.npcObjects.Count;
        Customer selectedNpc;
        //Debug.Log("Started Checking");
        do
        {
            customerDrawIndex = Random.Range(0, availableNpcCount);
            selectedNpc = NpcObjects.npcObjects[customerDrawIndex].GetComponent<Customer>();
        }
        while (selectedNpc.IsCustomer);
            
        standZone.AllotedObject = selectedNpc.gameObject;
        selectedNpc.SetAsCustomer();
        selectedNpc.GetComponent<NPC>().StandzoneTarget = standZone.transform;
        selectedNpc.GetComponent<NPC>().SetState(NpcState.MOVE_TO_VENDOR);
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
