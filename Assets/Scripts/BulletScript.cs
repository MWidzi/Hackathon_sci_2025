using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class BulletScript : MonoBehaviour
{
    public Vector2 direction;
    public float savedAngle;
    public int bulletSpeed = 7;
    public float spawnTime;
    public Vector2 moving;
    private bool isCaught = false;
    private Vector3 initialOffset;
    public bool redirected = false;

    public float activationRange = 3f;
    public float maxAttraction = 8f;
    public float chargeSpeed = 3f;
    public float releaseSpeed = 10f;

    public Color newColor;

    private Rigidbody2D rb;
    private float currentAttraction = 0f;

    Coroutine stopSlowmo;

    public AudioClip doorClip;
    public AudioClip deathClip;

    private IEnumerator terminateSlowmo()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.1f);
        currentAttraction = Mathf.MoveTowards(currentAttraction, 0f, Time.deltaTime * releaseSpeed);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = moving * bulletSpeed;
    }

    void FixedUpdate()
    {
        if (currentAttraction > 0f)
        {
            Vector2 dir = (GameManager.Instance.mainCam.transform.position - transform.position).normalized;
            rb.AddForce(dir * currentAttraction * 100f, ForceMode2D.Force);
        }

        rb.linearVelocity = rb.linearVelocity.normalized * bulletSpeed;
    }
    void Update()
    {
        bool isHolding = GameManager.Instance.holdAction.IsPressed();
        float distance = Vector2.Distance(transform.position, GameManager.Instance.mainCam.transform.position);

        if (distance < activationRange && isHolding && (GameManager.magnetizedBullet == null || GameManager.magnetizedBullet == this))
        {
            GameManager.magnetizedBullet = this;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.2f, 0.5f);
            GameManager.Instance.volume.profile = GameManager.Instance.profile2;
            if (stopSlowmo != null)
            {
                StopCoroutine(stopSlowmo);
            }
            stopSlowmo = StartCoroutine(terminateSlowmo());
            currentAttraction += chargeSpeed * Time.deltaTime;
            currentAttraction = Mathf.Clamp(currentAttraction, 0f, maxAttraction);
        }
        else
        {
            if (GameManager.magnetizedBullet == this)
            {
                GameManager.magnetizedBullet = null;
                isCaught = false;
                redirected = true;

                GetComponent<SpriteRenderer>().color = newColor;
                GameManager.Instance.player.GetComponent<PlayerController>().catched = false;
            }
            GameManager.Instance.volume.profile = GameManager.Instance.profile1;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.1f);
            currentAttraction = Mathf.MoveTowards(currentAttraction, 0f, Time.deltaTime * releaseSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Time.timeScale = 1f;

        if (collision.gameObject.layer == 3 && redirected)
        {
            collision.gameObject.SetActive(false);
            SoundController.Instance.PlayAudio(doorClip);
        }

        if (collision.gameObject.layer == 7)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isCaught)
        {
            GameManager.Instance.player.GetComponent<PlayerController>().catched = false;
        }

        Destroy(this.gameObject);
    }
}
