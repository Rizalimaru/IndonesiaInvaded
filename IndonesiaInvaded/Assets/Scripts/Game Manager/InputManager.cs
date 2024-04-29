using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private bool exitPressed = false;

    public static InputManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }
    public void ExitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            exitPressed = true;
        }
        else if (context.canceled)
        {
            exitPressed = false;
        }
    }

    public bool GetExitPressed() 
    {
        bool result = exitPressed;
        RegisterExitPressedThisFrame();
        return result;
    }

    public void RegisterExitPressedThisFrame() 
    {
        exitPressed = false;
    }

}
