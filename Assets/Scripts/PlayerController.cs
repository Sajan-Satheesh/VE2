using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] KeyCode upward, downward, leftward, rightward;
    [SerializeField] float speed;
    private Vector3 movement;
    [SerializeField] GameObject bullet;
    [SerializeField] Camera cam;
    [SerializeField] GameObject GameOverScreen;
    private Vector3 pointerPoistion;
    [SerializeField] RectTransform ShopArea;
    private Bounds ShootingBound;
    private Animator PlayerMovementAnimation;
    private Bounds playerMaxBoundary;
    [SerializeField] private Sprite defaultCursor;

    private void Awake()
    {
        PlayerMovementAnimation = GetComponent<Animator>();
    }

    private void Start()
    {
        playerMaxBoundary = WorldManager.mapBoundary;
        ShootingBound =new Bounds(Vector3.zero, ShopArea.sizeDelta) ;
    }

    private void CursorUpdate()
    {
        pointerPoistion = cam.ScreenToWorldPoint(Input.mousePosition);
        pointerPoistion.z = 0;
        if (!ShootingBound.Contains(pointerPoistion))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor.texture, Vector2.zero, CursorMode.Auto);
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
            CursorUpdate();
            Walk();
        }
    }
}
