using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class PointerLocationUpdate : UnityEvent<Vector3>
{
    
}

[System.Serializable]
public class PointerHide : UnityEvent
{
    
}

public class PointerLocation : MonoBehaviour
{
    [SerializeField] public PointerLocationUpdate pointerEvent;
    [SerializeField] public PointerHide pointerHideEvent;
    [SerializeField] public LayerMask pointerLayers;
    [SerializeField] public Camera cam;
    [SerializeField] private InventoryManager inventoryManager;

    private void LateUpdate()
    {
        if (inventoryManager.Active)
        {
            pointerHideEvent.Invoke();
            return;
        }

        var screenPosition = Mouse.current.position.ReadValue();
        var ray = cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out var hit, float.MaxValue, pointerLayers))
        {
            pointerEvent.Invoke(hit.point);
        }
    }
}
