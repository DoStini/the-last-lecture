
using UnityEngine;

public class ReleaseDamagePopup : MonoBehaviour
{
    public ObjectPool damagePopupPool;
    
    public void DestroyParent()
    {
        damagePopupPool.Release(gameObject.transform.parent.gameObject);
    }
}