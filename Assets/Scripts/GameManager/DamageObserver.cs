using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageObserver : MonoBehaviour
{
    /**
     * Sorted list of color thresholds. If damage passes a certain threshold, a different color is used.
     */
    public List<int> colorThresholds;
    /**
     * Number of colors is the size of color thresholds + 1. The first color is the default color, the second color is
     * used when damage passes the first threshold and so on.
     */
    public List<Color32> colors;
    public ObjectPool damagePopupPool;
    public float size = 1;

    private Vector3 _scale;

    private void Start()
    {
        _scale = new Vector3(size, 1, size);
    }

    private Color32 GetColor(int damage)
    {
        int index = colorThresholds.BinarySearch(damage);
        if (index < 0)
        {
            index = ~index;
        }

        return colors[index];
    }

    public void HandleDamagePopup(Character character, int damageTaken)
    {
        Vector3 characterPosition = character.transform.position;
        characterPosition.y += 1;
        damagePopupPool.GetAndActivate((o =>
        {
            o.transform.position = characterPosition;
            o.transform.localScale = _scale;
            ReleaseDamagePopup releaseDamagePopup = o.transform.GetChild(0).GetOrAddComponent<ReleaseDamagePopup>();
            releaseDamagePopup.damagePopupPool = damagePopupPool;
            
            Color32 color = GetColor(damageTaken);
            TextMeshPro textMesh = o.transform.GetChild(0).GetComponent<TextMeshPro>();
            textMesh.text = Mathf.Abs(damageTaken).ToString();
            textMesh.faceColor = color;
        }));
    }
}