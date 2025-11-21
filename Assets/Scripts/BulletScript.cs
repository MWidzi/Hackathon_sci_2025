using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector2 direction;
    public int bulletSpeed;

    void FixedUpdate()
    {
        this.gameObject.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
    }
}
