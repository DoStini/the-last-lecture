using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class PointerLocationUpdate : UnityEvent<Vector3>
{
    
}

public class PointerLocation : MonoBehaviour
{
    public PointerLocationUpdate pointerEvent;
    public LayerMask pointerLayers;
    public Camera cam;

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
