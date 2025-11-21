using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    }

    void FixedUpdate()
    {
        GameManager.Instance.playerRb.linearVelocity = moveValue * speed;
    }
}
