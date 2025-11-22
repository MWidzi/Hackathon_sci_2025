using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 7.0f;

    InputAction moveAction;
    InputAction dashAction;
    Vector2 moveValue;
    float dashActionTime = 0.1f;
    bool isDashing = false;
    float counter = 0;
    Vector3 dashDirection;
    void Start()
    {
        moveAction = Utilities.EnableAction("Move");
        dashAction = InputSystem.actions.FindAction("Dash");
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        dashDirection = GameManager.Instance.cameraController.direction.normalized;
    }

    void Dash(){
        print("e");
        counter += Time.deltaTime;
        GameManager.Instance.playerRb.linearVelocity = dashDirection * 20;
        if(counter >= dashActionTime){
            counter = 0;
            isDashing = false;
        }
    }
    void FixedUpdate()
    {
        this.gameObject.transform.rotation = GameManager.Instance.playerRotation;
        if (!GameManager.Instance.holdAction.IsPressed() && !isDashing)
            GameManager.Instance.playerRb.linearVelocity = moveValue * speed;
        if(dashAction.IsPressed()){
            isDashing = true;
        }
        if(isDashing){
            Dash();
        }

        GameManager.Instance.playerRb.linearVelocity = moveValue * speed;
    }
}
