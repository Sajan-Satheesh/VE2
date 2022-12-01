
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance { get; set; }
    public int HighScore;
    public int MaxGameTime;

    private void Awake()
    {
        HighScore = 0;
        if (GameManagerInstance!=null && GameManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            GameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
}


