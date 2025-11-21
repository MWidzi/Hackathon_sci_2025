using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    InputAction aimAction;
    Vector2 screenPos;
    public Vector3 mouseWorldPos;
    Vector3 offset;

    public Vector3 direction;

    Vector3 targetCamPos;

    public float maxDistance = 5f;
    public float offsetLimit = 0.2f;

    private float playerAngle;

    void Start() { aimAction = Utilities.EnableAction("Aim"); }

    void Update()
    {
        // camera between player and mouse pos in world units
        screenPos = aimAction.ReadValue<Vector2>();
        mouseWorldPos = GameManager.Instance.mainCam.GetComponent<Camera>().ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, GameManager.Instance.mainCam.transform.position.z * -1));

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        direction = mouseWorldPos - playerPos;
        playerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameManager.Instance.playerRotation = Quaternion.Euler(0, 0, playerAngle);

        offset = direction.normalized * direction.magnitude * offsetLimit;

        targetCamPos = playerPos + offset;
        targetCamPos.z = -10f;

        GameManager.Instance.mainCam.transform.position = targetCamPos;
    }
}
