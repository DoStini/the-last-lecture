using System.Collections.Generic;
using UnityEngine;

public class DamageObserver : MonoBehaviour
{
    public List<float> colorThresholds;
    public List<Color> colors;

    public void HandleDamagePopup(Character character, int damageTaken)
    {
        Debug.Log(character + " took " + damageTaken);
    }
}