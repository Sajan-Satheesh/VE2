using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] KeyCode upward, downward, leftward, rightward;
    [SerializeField] float speed;
    [SerializeField] float shootSpeed;
    private Vector3 movement;
    [SerializeField] Transform marker;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject copybullet;
    [SerializeField] Camera cam;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] bool fire;
    private Vector3 shootAim;
    private Vector3 shootDirection;
    [SerializeField] RectTransform ShopArea;
    private Bounds ShootingBound;
    private Animator PlayerMovementAnimation;
    private Bounds playerMaxBoundary;

    private void Awake()
    {
        PlayerMovementAnimation = GetComponent<Animator>();
    }

    private void Start()
    {
        playerMaxBoundary = WorldManager.mapBoundary;
        fire = false;
        copybullet = null;
        ShootingBound =new Bounds(Vector3.zero, ShopArea.sizeDelta) ;
    }
    void shoot()
    {
        shootAim = cam.ScreenToWorldPoint(Input.mousePosition);
        shootAim = new Vector3(shootAim.x, shootAim.y, 0);

        if (!fire)
        {
            shootDirection = (shootAim - transform.position).normalized;
        }

        if (Input.GetMouseButtonDown(0)&&ShootingBound.Contains(shootAim))
        {
            if (copybullet==null)
            {
                copybullet = Instantiate(bullet, transform.position, Quaternion.FromToRotation(Vector3.up, shootDirection));
                fire = true;
            }
        }
        if (!ShootingBound.Contains(shootAim))
        {
            Cursor.visible = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.visible = false;
            marker.position = shootAim;
        }
        if (fire)
        {
            copybullet.transform.position += shootDirection * Time.deltaTime*shootSpeed;
        }
        
    }

    void bulletRestore()
    {
        if (copybullet!=null && !ShootingBound.Contains(copybullet.transform.position))
        {
            Destroy(copybullet);
            fire = false;
        }
    }
    void Walk()
    {
        movement = new Vector3(0,0,0);
        if (Input.GetKey(upward)&&PlayerMovementAnimation.isInitialized)
        {
            PlayerMovementAnimation.SetInteger("Directions", -1);
            movement += Vector3.up;
        }
        else if (Input.GetKey(downward) && PlayerMovementAnimation.isInitialized)
        {
            PlayerMovementAnimation.SetInteger("Directions", 1);
            movement += Vector3.down;
        }
        else if (Input.GetKey(leftward) && PlayerMovementAnimation.isInitialized)
        {
            PlayerMovementAnimation.SetInteger("Directions", 2);
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            movement += Vector3.left;
        }
        else if (Input.GetKey(rightward) && PlayerMovementAnimation.isInitialized)
        {
            PlayerMovementAnimation.SetInteger("Directions", 2);
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            movement += Vector3.right;
        }
        else PlayerMovementAnimation.SetInteger("Directions", 0);
        transform.position += speed * Time.deltaTime * movement.normalized;
    }

    void AbandonShop()
    {
        if(!playerMaxBoundary.Contains(gameObject.transform.position))
        {
            GameOverScreen.SetActive(true);
        }
    }
    void Update()
    {
        AbandonShop();
        if (!WorldManager.pause)
        {
            shoot();
            bulletRestore();
            Walk();
        }
    }
}
