using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Quaternion direction;
    public float savedDirection;
    public int bulletSpeed = 7;
    public float spawnTime;
    public Vector2 moving;
    private bool isCaught = false;

    void Start()
    {
        this.gameObject.transform.rotation = direction;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.holdAction.IsPressed() && this.gameObject.GetComponent<Collider2D>().IsTouching(GameManager.Instance.player.transform.GetChild(0).gameObject.GetComponent<Collider2D>()) && !isCaught)
        {
            moving = Vector2.zero;
            this.transform.parent = GameManager.Instance.player.transform;
            savedDirection = GameManager.Instance.player.transform.rotation.z;
            isCaught = true;
            Time.timeScale = 0.2f;
        }
        else if (!isCaught)
        {
            moving = transform.right;
            Time.timeScale = 1f;
        }

        this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = moving * bulletSpeed;
    }

    void Update()
    {
        if (GameManager.Instance.holdAction.WasReleasedThisDynamicUpdate() && isCaught)
        {
            this.gameObject.transform.parent = null;
            this.gameObject.transform.rotation *= Quaternion.Euler(0, 0, 90);
            isCaught = false;
        }


    }
}
