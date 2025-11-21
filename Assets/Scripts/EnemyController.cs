using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private Vector2 direction;
    private float angle;
    Quaternion rotation;

    private IEnumerator shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            GameManager.Instance.SpawnBullet(0, rotation, this.gameObject.transform);
        }
    }


    void Start()
    {
        StartCoroutine(shoot());
    }

    void FixedUpdate()
    {
        direction = (GameManager.Instance.player.transform.position - this.gameObject.transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0, 0, angle);
        this.transform.rotation = rotation;
    }
}
