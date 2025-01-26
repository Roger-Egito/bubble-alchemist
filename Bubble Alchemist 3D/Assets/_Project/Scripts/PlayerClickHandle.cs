using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerClickHandle : MonoBehaviour
{
    public void PopBalloon (InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        //RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 1000)) return;

        //if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<Bubble>(out Bubble bubble))
        {
            Debug.Log("POP!");
            bubble.Pop();
        }
    }
}
