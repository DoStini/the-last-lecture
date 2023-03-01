
using UnityEngine;

public class DestroyParentAnimationEnd : MonoBehaviour
{
    public void DestroyParent()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}