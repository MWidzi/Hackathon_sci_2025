using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public Rigidbody2D playerRb;

    public GameObject mainCam;
    public CameraController cameraController;

    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();

        cameraController = mainCam.GetComponent<CameraController>();
    }
}
