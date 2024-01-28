
using System.Collections;
using UnityEngine;

public class NPC : NPCorientation
{
    [SerializeField] private float npcSpeed;
    [SerializeField] private float directionRayLength;
    [SerializeField] private float adjustmentSpeed;
    [SerializeField] GameObject face;
    private Vector2 tablePosition;
    private Animator npcAnimator;
    private SpriteRenderer npcRenderer;
    private Vector3 moveTowardsVector { get; set; }
    public bool resting;
    public NpcState currentState;
    public Direction npcDirection;
    public Transform StandzoneTarget;
    public Transform SeatTarget;

    private void Awake()
    {
        currentState = NpcState.MOVING;
        StandzoneTarget = null;
        SeatTarget = null;
        npcAnimator = GetComponent<Animator>();
        npcRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        MapBoundary.size = WorldManager.mapBoundary.size;
        tablePosition = WorldManager.tablePosition;
    }

    public void ResetCustomer()
    {
        StandzoneTarget = null;
        SeatTarget = null;
        GetComponent<Customer>().IsCustomer = false;
        resting = false;
        SetRandomDirection();
        SetState(NpcState.MOVING);
    }
    public void SetRandomDirection()
    {
        int randomDirection = Random.Range(0, 2);
        npcAnimator.SetBool("standZone",false);
        if (randomDirection == 0)
        {
            npcDirection = Direction.left;
        }
        else npcDirection = Direction.right;
    }
    
    void RenderAboveTables()
    {
        if (transform.position.y < tablePosition.y)
        {
            npcRenderer.sortingOrder = 5;
        }
        else 
        {
            npcRenderer.sortingOrder = 1;
        } 
    }
    public void SetState(NpcState state)
    {
        currentState = state;
    }
    void UpdateAsPerState()
    {
        switch (currentState)
        {
            case NpcState.EATING:
                RestingState();
                break;
            case NpcState.MOVING:
                Movement();
                break;
            case NpcState.MOVE_TO_VENDOR:
                MoveToTarget(StandzoneTarget);
                break;
            case NpcState.MOVE_TO_TABLE:
                MoveToTarget(SeatTarget);
                break;
            default:
                break;
        }
    }

    private void RestingState()
    {
        npcAnimator.SetBool("walk", false);
        npcAnimator.SetInteger("Towards", 0);
        if (npcDirection == Direction.up)
        {
            npcAnimator.SetBool("standZone", true);
        }
        else if (npcDirection == Direction.down)
        {
            npcAnimator.SetBool("Seat12", true);
        }
    }
    void Update()
    {
        RenderAboveTables();
        UpdateAsPerState();
    }

    private void MoveToTarget(Transform target)
    {
        npcAnimator.SetBool("walk", false);
        if (!resting)
        {
            if (face.transform.position.y > 0)
            {
                npcAnimator.SetInteger("Towards", 2);
            }
            else if (face.transform.position.y < 0)
            {
                npcAnimator.SetInteger("Towards", 1);
            }
        }
        face.transform.LookAt(target,Vector3.forward);
        if (Vector3.Distance(transform.position, target.position) > 0.01)
        {
            transform.Translate(face.transform.forward * Time.deltaTime * npcSpeed);
        }
    }

    void Movement()
    {
        npcAnimator.SetInteger("Towards", 0);
        npcAnimator.SetBool("walk", true);
        moveTowardsVector = new Vector3(0,0,0);
        switch (npcDirection)
        {
            case Direction.right: 
                moveTowardsVector = Vector3.left;
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                break;

            case Direction.left: 
                moveTowardsVector = Vector3.right;
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                break;
        }
        transform.position += npcSpeed * Time.deltaTime * moveTowardsVector;
    }
   
    private void FixedUpdate()
    {
        CheckBoundary();
        if (!GetComponent<Customer>().IsCustomer)
        {
            SafeWalk();
        }
    }

    void CheckBoundary()
    {
        Vector2 position = transform.position;
        if (position.x > MaxBound.x || position.x < MinBound.x || position.y > MaxBound.y || position.y < MinBound.y)
        {
            transform.position = SpawnRandomInBounds(MinBound, MaxBound);
            npcDirection = GetNpcDirection(transform.position);
        }
    }

    void SafeWalk()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveTowardsVector,directionRayLength, LayerMask.GetMask("NPC","Private"));
        
        if (hit.collider!=null)
        {
            Debug.DrawRay(transform.position, moveTowardsVector * hit.distance, Color.red);
            float adjustThreshold = Random.Range(1, 5);
            float distanceBetween = hit.distance;
            if (adjustmentSpeed < 5 )
            {
                adjustmentSpeed = 1 / (distanceBetween*adjustThreshold);
            }
            else adjustmentSpeed = 0;

            if(transform.position.y < hit.transform.position.y)
            {
                transform.position += adjustmentSpeed * Time.deltaTime * Vector3.down;
            }
            else transform.position += adjustmentSpeed * Time.deltaTime * Vector3.up;
        }
        
        else
        {
            Debug.DrawRay(transform.position, moveTowardsVector * directionRayLength, Color.white);
        }

    }

}
public enum NpcState { EATING, MOVING, MOVE_TO_VENDOR, MOVE_TO_TABLE}