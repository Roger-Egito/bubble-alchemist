using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class PlayerC : MonoBehaviour
{
    public void PopBubble(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<Bubble>(out Bubble bubble))
        {
            bubble.Pop();
        }
    }
}
