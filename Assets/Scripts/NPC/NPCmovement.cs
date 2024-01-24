
using System.Collections;
using UnityEngine;

public class NPCMovement : NPCorientation
{
    [SerializeField] private float npcSpeed;
    [SerializeField] private float directionRayLength;
    [SerializeField] private float adjustmentSpeed;
    [SerializeField] GameObject face;
    private Vector2 tablePosition;
    private Animator npcAnimator;
    private SpriteRenderer npcRenderer;
    private Vector3 npcDirection;
    public bool resting;
    public bool IsCustomer;
    public Direction npcStartDirection;
    public Transform StandzoneTarget;
    public Transform SeatTarget;

    private void Awake()
    {
        IsCustomer = false;
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
    private void OnEnable()
    {
        OrderItem.CancelOrder += Leave;
    }
    public void SetDirection()
    {
        StandzoneTarget = null;
        SeatTarget = null;
        int randomDirection = Random.Range(0, 2);
        npcAnimator.SetBool("standZone",false);
        if (randomDirection == 0)
        {
            npcStartDirection = Direction.left;
        }
        else npcStartDirection = Direction.right;
    }
    
    void RenderAboveTables()
    {
        if(transform.position.y< tablePosition.y)
        {
            npcRenderer.sortingOrder = 4;
        }
        else npcRenderer.sortingOrder = 3;
    }

    void Update()
    {
        RenderAboveTables();
        if (StandzoneTarget == null && SeatTarget == null)
        {
            Movement();
        }
        else if (StandzoneTarget != null)
        {
            MoveToTarget(StandzoneTarget);
        }
        else  if(SeatTarget != null)
        {
            MoveToTarget(SeatTarget);
        }
        if (resting)
        {
            npcAnimator.SetBool("walk", false);
            npcAnimator.SetInteger("Towards", 0);
            if (npcStartDirection == Direction.up)
            {
                npcAnimator.SetBool("standZone", true);
            }
            else if (npcStartDirection == Direction.down)
            {
                npcAnimator.SetBool("Seat12", true);
            }
        }
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
        npcDirection = new Vector3(0,0,0);
        switch (npcStartDirection)
        {
            case Direction.right: 
                npcDirection = Vector3.left;
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                break;

            case Direction.left: 
                npcDirection = Vector3.right;
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                break;
        }
        transform.position += npcSpeed * Time.deltaTime * npcDirection;
    }
   
    private void FixedUpdate()
    {
        CheckBoundary();
        if (!IsCustomer)
        {
            SafeWalk();
        }
    }

    void CheckBoundary()
    {
        Vector2 position = transform.position;
        if (position.x > MaxBound.x || position.x < MinBound.x || position.y > MaxBound.y || position.y < MinBound.y)
        {
            NPCdirection(transform.position, gameObject.GetComponent<NPCMovement>());
            transform.position = InstantiateLocation(MinBound, MaxBound);
        }
    }

    void SafeWalk()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, npcDirection,directionRayLength, LayerMask.GetMask("NPC","Private"));
        
        if (hit.collider!=null)
        {
            Debug.DrawRay(transform.position, npcDirection * hit.distance, Color.red);
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
            Debug.DrawRay(transform.position, npcDirection * directionRayLength, Color.white);
        }

    }

    private void Leave(Customer customer)
    {
        StartCoroutine(ClearCustomerInfo(customer));
    }
    private IEnumerator ClearCustomerInfo(Customer leavingCustomer)
    {
        if (gameObject.name == leavingCustomer.gameObject.name)
        {
            yield return new WaitForSeconds(1);

            if (StandzoneTarget!=null)
            {
                resting = false;
                StandzoneTarget = null;
                SetDirection();
            }
            else if (SeatTarget!=null)
            {
                resting = false;
                SeatTarget = null;
                IsCustomer = false;
            }
            SetDirection();
            leavingCustomer.npcCanvasBar.SetActive(false);
            leavingCustomer.fillAmount = 0f;
        }
        StopCoroutine(ClearCustomerInfo(leavingCustomer));
    }

    private void OnDisable()
    {
        OrderItem.CancelOrder -= Leave;
    }


}
