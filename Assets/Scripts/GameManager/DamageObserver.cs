using System;
using System.Collections.Generic;
using TMPro;
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
    public GameObject damageText;

    private Color32 getColor(int damage)
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
        Debug.Log("------");
        Debug.Log(character + " took " + damageTaken);

        Vector3 characterPosition = character.transform.position;
        characterPosition.y += 1;
        GameObject damageInst = Instantiate(damageText.gameObject, characterPosition, Quaternion.identity);
        Color32 color = getColor(damageTaken);
        TextMeshPro textMesh = damageInst.transform.GetChild(0).GetComponent<TextMeshPro>();
        textMesh.text = Mathf.Abs(damageTaken).ToString();
        textMesh.faceColor = color;
    }
}