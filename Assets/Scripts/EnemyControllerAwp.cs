using UnityEngine;
using System.Collections;

public class EnemyControllerAwp : MonoBehaviour
{
    private Vector2 direction;
    private float angle;
    private Quaternion rotation;

    public float range = 1f;  // distance where enemy stops using A*
    public float speed = 3f;

    private PathRenderer pathRenderer;
    private Pathfinding pathfinding;
    private LineRenderer lineRenderer;

    void Start()
    {
        // components unique to this enemy
        pathRenderer = GetComponent<PathRenderer>();
        pathfinding = GetComponent<Pathfinding>();

        // assign seeker/target
        pathfinding.seeker = this.transform;
        pathfinding.target = GameManager.Instance.player.transform;

        // setup line renderer for laser sight
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

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
            // Wait before telegraphing the shot. The whole cycle is ~3s.
            yield return new WaitForSeconds(1f);

            // Telegraph: 1s solid red line, then 1s blinking.
            lineRenderer.enabled = true;
            yield return new WaitForSeconds(1f);

            // Blinking for 1 second
            float blinkEndTime = Time.time + 1f;
            while (Time.time < blinkEndTime)
            {
                lineRenderer.enabled = !lineRenderer.enabled;
                yield return new WaitForSeconds(0.05f); // Fast blink
            }

            lineRenderer.enabled = false; // Ensure it's off

            GameObject bullet = GameManager.Instance.SpawnBullet(0, direction, transform);
            bullet.GetComponent<BulletScript>().bulletSpeed = 20;
            // bullet.GetComponent<Collider2D>().excludeLayers = LayerMask.GetMask("walls");
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

        if (lineRenderer.enabled)
        {
            Vector3 startPos = transform.position;
            Vector3 playerPos = GameManager.Instance.player.transform.position;

            lineRenderer.SetPosition(0, startPos);

            Vector2 directionToPlayer = (playerPos - startPos).normalized;
            float distanceToPlayer = Vector2.Distance(startPos, playerPos);
            int wallLayerMask = LayerMask.GetMask("walls");

            RaycastHit2D hit = Physics2D.Raycast(startPos, directionToPlayer, distanceToPlayer, wallLayerMask);

            if (hit.collider != null)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, playerPos);
            }
        }
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
