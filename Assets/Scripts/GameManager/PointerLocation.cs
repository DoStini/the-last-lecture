using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class PointerLocationUpdate : UnityEvent<Vector3>
{
    
}

public class PointerLocation : MonoBehaviour
{
    [SerializeField] public PointerLocationUpdate pointerEvent;
    [SerializeField] public LayerMask pointerLayers;
    [SerializeField] public Camera cam;

    private void LateUpdate()
    {
        var screenPosition = Mouse.current.position.ReadValue();
        var ray = cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out var hit, float.MaxValue, pointerLayers))
        {
            pointerEvent.Invoke(hit.point);
        }
    }
}
