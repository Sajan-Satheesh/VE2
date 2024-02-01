
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NPC : NPCorientation
{
    [SerializeField] private float npcSpeed;
    [SerializeField] private float directionRayLength;
    [SerializeField] private float adjustmentSpeed;
    [field: SerializeField] public GameObject face { get; private set; }

    private Vector2 tablePosition;
    private Animator npcAnimator;
    private SpriteRenderer npcRenderer { get; set; }
    public bool resting;
    public NpcState currentState;
    public NpcAnimationsStates currentAnimationState;
    private Collider2D npcCollider;
    public Transform StandzoneTarget { private get; set; }
    public Transform SeatTarget { private get; set; }
    public Vector3 forwardDirection = Vector3.zero;

    private void Awake()
    {
        currentState = NpcState.MOVING;
        StandzoneTarget = null;
        SeatTarget = null;
        npcAnimator = GetComponent<Animator>();
        npcRenderer = GetComponent<SpriteRenderer>();
        npcCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        MapBoundary.size = WorldManager.mapBoundary.size;
        tablePosition = WorldManager.tablePosition;
    }

    public void ResetCharacter()
    {
        npcRenderer.sortingOrder = 3;
        npcCollider.enabled = true;
        ResetUsedStandZone();
        ResetUsedSeat();
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
            forwardDirection = Vector3.left;
        }
        else forwardDirection = Vector3.right;
    }
    
    void RenderAboveTables()
    {
        if (transform.position.y < tablePosition.y)
        {
            npcRenderer.sortingOrder = resting? 6 : 7;
        }
        else 
        {
            npcRenderer.sortingOrder = resting ? 4 : 3;
        } 
    }
    public void SetForwardDirection(Vector3 direction)
    {
        forwardDirection = direction;
        forwardDirection.z = 0f;
    }
    public void SetAnimationState(NpcAnimationsStates animationState)
    {
        if(currentAnimationState == animationState) return;
        currentAnimationState = animationState;
        npcAnimator.Play(currentAnimationState.ToString(),0);
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
                EatingState();
                break;
            case NpcState.MOVING:
                Movement();
                break;
            case NpcState.MOVE_TO_VENDOR:
                MoveToTarget(StandzoneTarget, NpcState.WAITING);
                break;
            case NpcState.WAITING:
                WaitingState();
                break;
            case NpcState.MOVE_TO_TABLE:
                MoveToTarget(SeatTarget, NpcState.EATING);
                break;
            default:
                break;
        }
    }


    void Update()
    {
        RenderAboveTables();
        UpdateAsPerState();
        UpdateAnimations();
    }

    bool yGreaterThanX() => Mathf.Abs(forwardDirection.y) > Mathf.Abs(forwardDirection.x);
    private void EatingState()
    {
        if (resting) return;

        npcCollider.enabled = false;
        forwardDirection = (transform.position.y < tablePosition.y) ? Vector3.up : Vector3.down;
        GetComponent<Customer>().StartToEat();
        resting = true;
    }
    private void WaitingState()
    {
        if (resting) return;

        SetForwardDirection((WorldManager.stallTransform.position - transform.position).normalized);
        resting = true;
    }
    void UpdateAnimations()
    {
        if (yGreaterThanX() && forwardDirection.y < 0)
        {
            if (resting)
            {
                SetAnimationState(NpcAnimationsStates.NpcFaceDown);
                return;
            }
            SetAnimationState(NpcAnimationsStates.NpcWalkDown);
            return;
        }
        else if (yGreaterThanX() && forwardDirection.y > 0)
        {
            if (resting)
            {
                SetAnimationState(NpcAnimationsStates.NpcFaceUp);
                return;
            }
            SetAnimationState(NpcAnimationsStates.NpcWalkUp);
            return;
        }
        else if (!yGreaterThanX() && forwardDirection.x < 0)
        {
            SetAnimationState(NpcAnimationsStates.NpcWalkLeft);
            return;
        }
        else if (!yGreaterThanX() && forwardDirection.x > 0)
        {
            SetAnimationState(NpcAnimationsStates.NpcWalkRight);
            return;
        }
    }
    private void MoveToTarget(Transform target, NpcState nextState)
    {
        if (currentState == NpcState.MOVE_TO_TABLE) ResetUsedStandZone();
        if(resting) resting = false;

        SetForwardDirection((target.position - transform.position).normalized);

        if (Vector3.Distance(transform.position, target.position) > 0.05)
        {
            transform.Translate(forwardDirection * Time.deltaTime * npcSpeed);
        }
        else
        {
            if (currentState == NpcState.MOVE_TO_VENDOR) GetComponent<Customer>().placeOrder();
            if (GetComponent<Customer>().IsCustomer) SetState(nextState);
        }
    }

    void Movement()
    {
        ResetUsedSeat();
        ResetUsedStandZone();

        if (resting) resting = false;

        transform.position += npcSpeed * Time.deltaTime * forwardDirection;
    }

    private void ResetUsedSeat()
    {
        if (SeatTarget != null)
        {
            SeatTarget.GetComponent<Chair>().ResetChair();
            SeatTarget = null;
        }
    }
    private void ResetUsedStandZone()
    {
        if (StandzoneTarget != null)
        {
            StandzoneTarget.GetComponent<StandZone>().ResetStandZone();
            StandzoneTarget = null;
        }
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
            transform.position = SpawnRandomInBounds();
            SetForwardDirection(GetSpawnNpcDirection(transform.position));
        }
    }

    void SafeWalk()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, forwardDirection, directionRayLength, LayerMask.GetMask("NPC","Private"));
        
        if (hit.collider!=null)
        {
            Debug.DrawRay(transform.position, forwardDirection * hit.distance, Color.red);
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
            Debug.DrawRay(transform.position, forwardDirection * directionRayLength, Color.white);
        }

    }

}
public enum NpcState { EATING, MOVING, MOVE_TO_VENDOR, MOVE_TO_TABLE, WAITING}
public enum NpcAnimationsStates { NpcWalkLeft, NpcWalkRight, NpcWalkUp, NpcWalkDown, NpcFaceUp, NpcFaceDown }