using UnityEngine;
using System.Collections;

public class Leve2Controller : MonoBehaviour {
    public const int GRID_SIZE = 1;

    public static Sprite RED_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks1");
    public static Sprite ORANGE_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks2");
    public static Sprite YELLOW_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks3");
    public static Sprite GREEN_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks4");
    public static Sprite CYAN_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks5");
    public static Sprite BLUE_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks6");
    public static Sprite PURPLE_CUBE = Resources.Load<Sprite>("Sprites/buildable_blocks7");

    private static Leve2Controller instance;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


}
