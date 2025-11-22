using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private Vector2 direction;
    private float angle;
    private Quaternion rotation;

    public float range = 1f;  // distance where enemy stops using A*
    public float speed = 3f;

    private PathRenderer pathRenderer;
    private Pathfinding pathfinding;

    void Start()
    {
        // components unique to this enemy
        pathRenderer = GetComponent<PathRenderer>();
        pathfinding = GetComponent<Pathfinding>();

        // assign seeker/target
        pathfinding.seeker = this.transform;
        pathfinding.target = GameManager.Instance.player.transform;

        // start path updates + shooting
        StartCoroutine(UpdatePathRoutine());
        StartCoroutine(ShootRoutine());
    }

    // update the path every 0.2 seconds (fast and cheap)
    IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            pathfinding.FindPath(pathfinding.seeker.position, pathfinding.target.position);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.SpawnBullet(0, direction, this.gameObject.transform);
        }
    }

    void FixedUpdate()
    {
        // rotate toward player
        direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0, 0, angle + 90);
        transform.rotation = rotation;
    }

    void Update()
    {
        FollowPath();
    }

    void FollowPath()
    {
        float distToPlayer = Vector2.Distance(transform.position, GameManager.Instance.player.transform.position);

        // If very close to player, stop using A* and just rotate/shoot
        if (distToPlayer < range)
            return;

        if (pathRenderer.currentPath == null || pathRenderer.currentPath.Count == 0)
            return;

        Vector2 target = pathRenderer.currentPath[0].worldPos;
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
            pathRenderer.currentPath.RemoveAt(0);
    }
}
