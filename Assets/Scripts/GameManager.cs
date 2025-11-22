using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static BulletScript magnetizedBullet;

    public GameObject player;
    public Rigidbody2D playerRb;
    public Quaternion playerRotation;

    public GameObject mainCam;
    public CameraController cameraController;

    public GameObject[] bulletPrefabs;

    public InputAction holdAction;

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

    public void SpawnBullet(int prefabId, Vector2 bulletDirection, Transform parent)
    {
        GameObject bullet = Instantiate(GameManager.Instance.bulletPrefabs[prefabId]);
        bullet.transform.position = parent.position;
        bullet.GetComponent<BulletScript>().moving = bulletDirection;
    }

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();

        cameraController = mainCam.GetComponent<CameraController>();
        holdAction = Utilities.EnableAction("Hold");

    }
}
