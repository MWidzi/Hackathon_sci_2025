using UnityEngine;
using UnityEngine.InputSystem;

public class Utilities : MonoBehaviour
{
    public static InputAction EnableAction(string name)
    {
        InputAction action = InputSystem.actions.FindAction(name);
        action.Enable();

        return action;
    }
}
