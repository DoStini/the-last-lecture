using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    private ProgressBar _healthBar;
    private Label _currentAmmo;
    private Label _stock;
    private VisualElement _ammoCounter;
    
    // Start is called before the first frame update
    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        _healthBar = root.Q<ProgressBar>("HealthProgress");
        _currentAmmo = root.Q<Label>("CurrentAmmo");
        _stock = root.Q<Label>("Stock");
        _ammoCounter = root.Q<VisualElement>("AmmoCounter");
    }

    public void ToggleAmmo(bool active)
    {
        _ammoCounter.style.display = active ? 
            new StyleEnum<DisplayStyle>(DisplayStyle.Flex) : new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    public void UpdateCurrentAmmo(int ammo)
    {
        _currentAmmo.text = ammo.ToString();
    }

    public void UpdateStock(int stockCount)
    {
        _stock.text = stockCount.ToString();
    }

    public void UpdateMaxHealth(int maxHealth)
    {
        _healthBar.highValue = maxHealth;
    }

    public void UpdateHealth(int health)
    {
        _healthBar.value = health;
    }
}
