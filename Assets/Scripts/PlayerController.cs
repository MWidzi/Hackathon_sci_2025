using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool catched;

    public float speed = 7.0f;

    InputAction moveAction;
    Vector2 moveValue;

    void Start()
    {
        moveAction = Utilities.EnableAction("Move");
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();

        if (catched)
        {
            //Time.timeScale = 0.2f;
        }
        else
        {
            //Time.timeScale = 1f;
        }
    }

    void FixedUpdate()
    {
        if (!catched)
        {
            GameManager.Instance.playerRb.linearVelocity = moveValue * speed;
        }
        else
        {
            GameManager.Instance.playerRb.linearVelocity = Vector2.zero;
        }

        this.gameObject.transform.rotation = GameManager.Instance.playerRotation * Quaternion.Euler(0, 0, 90);
    }
}
