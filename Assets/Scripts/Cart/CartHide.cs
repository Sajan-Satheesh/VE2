
using TMPro;
using UnityEngine;

public class CartHide : MonoBehaviour
{
    SpriteRenderer CartHoodSprite;
    [SerializeField, Range(0,1)] float transparency;
    private Color orginalColor;

    private void Awake()
    {
        CartHoodSprite = GetComponent<SpriteRenderer>();
        orginalColor = CartHoodSprite.color;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController controller))
        {
            CartHoodSprite.color = new Color(orginalColor.r, orginalColor.g, orginalColor.b, transparency);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CartHoodSprite.color = orginalColor;
    }
}
