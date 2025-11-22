using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Quaternion direction;
    public float savedAngle;
    public int bulletSpeed = 7;
    public float spawnTime;
    public Vector2 moving;
    private bool isCaught = false;
    private Vector3 initialOffset;
    public bool firedByPlayer = false;

    void Start()
    {
        this.gameObject.transform.rotation = direction;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.holdAction.IsPressed() && this.gameObject.GetComponent<Collider2D>().IsTouching(GameManager.Instance.player.transform.GetChild(0).gameObject.GetComponent<Collider2D>()) && !isCaught)
        {
            moving = Vector2.zero;
            savedAngle = GameManager.Instance.player.transform.eulerAngles.z;
            isCaught = true;
            Time.timeScale = 0.2f;
            initialOffset = GameManager.Instance.player.transform.InverseTransformPoint(transform.position);
        }
        else if (!isCaught)
        {
            moving = transform.right;
            Time.timeScale = 1f;
        }

        if (isCaught)
        {
            this.transform.position = GameManager.Instance.player.transform.TransformPoint(initialOffset);
        }

        this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = moving * bulletSpeed;
    }

    void Update()
    {
        if (GameManager.Instance.holdAction.WasReleasedThisDynamicUpdate() && isCaught)
        {
            float rotationChange = Mathf.DeltaAngle(savedAngle, GameManager.Instance.player.transform.eulerAngles.z);
            float clampedRotationChange = Mathf.Clamp(rotationChange, -45f, 45f);
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, GameManager.Instance.player.transform.eulerAngles.z + clampedRotationChange);

            isCaught = false;
            firedByPlayer = true;
        }
    }
}