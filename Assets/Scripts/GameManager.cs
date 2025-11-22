using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


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

    public Volume volume;
    public VolumeProfile profile1;
    public VolumeProfile profile2;

    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        mainCam = Camera.main.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        cameraController = mainCam.GetComponent<CameraController>();

        GameObject[] sfxToDestroy = GameObject.FindGameObjectsWithTag("SFX");
        foreach (GameObject sfx in sfxToDestroy)
        {
            Destroy(sfx);
        }
        if (SceneManager.GetActiveScene().name == "Level 2")
            SoundController.Instance.PlayAudio(clip2, true);
        else if (SceneManager.GetActiveScene().name == "Level 3")
            SoundController.Instance.PlayAudio(clip3, true);
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // singleton
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public GameObject SpawnBullet(int prefabId, Vector2 bulletDirection, Transform parent)
    {
        GameObject bullet = Instantiate(GameManager.Instance.bulletPrefabs[prefabId]);
        bullet.transform.position = parent.position;
        bullet.GetComponent<BulletScript>().moving = bulletDirection;

        return bullet;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
            SoundController.Instance.PlayAudio(clip1, true);
        else if (SceneManager.GetActiveScene().name == "Level 2")
            SoundController.Instance.PlayAudio(clip2, true);
        else if (SceneManager.GetActiveScene().name == "Level 3")
            SoundController.Instance.PlayAudio(clip3, true);
        SceneManager.sceneLoaded += OnLoadScene;
        playerRb = player.GetComponent<Rigidbody2D>();

        DontDestroyOnLoad(volume);
        cameraController = mainCam.GetComponent<CameraController>();
        holdAction = Utilities.EnableAction("Hold");
    }

    private void FixedUpdate()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
