using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool catched;

    public float speed = 7.0f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.1f;

    InputAction moveAction;
    Vector2 moveValue;

    InputAction dashAction;

    private bool isDashing = false;

    public AudioClip dashClip;

    void Start()
    {
        moveAction = Utilities.EnableAction("Move");
        dashAction = Utilities.EnableAction("Dash");
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();

        if (dashAction.triggered && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            GameManager.Instance.playerRb.linearVelocity = moveValue * speed;
        }

        this.gameObject.transform.rotation = GameManager.Instance.playerRotation * Quaternion.Euler(0, 0, 90);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        Vector2 dashDirection = GameManager.Instance.cameraController.direction.normalized;
        GameManager.Instance.playerRb.linearVelocity = dashDirection * dashSpeed;

        SoundController.Instance.PlayAudio(dashClip);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }
}
