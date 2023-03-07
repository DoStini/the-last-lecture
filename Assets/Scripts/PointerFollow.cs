using UnityEngine;

public class PointerFollow : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(0, 10000, 0);;
    }

    public void UpdatePointerPosition(Vector3 pointerLocation)
    {
        gameObject.SetActive(true);
        transform.position = pointerLocation;
    }

    public void HidePointer()
    {
        gameObject.SetActive(false);
    }
}
