using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[Serializable]
public class RetryEvent : UnityEvent
{
    
}

public class HUDManager : MonoBehaviour
{
    [SerializeField] private RetryEvent retryEvent;
    [SerializeField] private Player player;
    public int maxHealth = 50;

    private ProgressBar _healthBar;
    private Label _currentAmmo;
    private Label _stock;
    private VisualElement _ammoCounter;
    private Label _currentSpeed;
    private VisualElement _speedCounter;
    private VisualElement _botBar;
    private VisualElement _skullHead;
    private VisualElement _deathMenu;
    private VisualElement _retryButton;
    private Label _currentHealth;
    private Label _maxHealth;

    // Start is called before the first frame update
    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        _healthBar = root.Q<ProgressBar>("HealthProgress");
        _currentHealth = root.Q<Label>("CurrentHealth");
        _maxHealth = root.Q<Label>("MaxHealth");
        _currentAmmo = root.Q<Label>("CurrentAmmo");
        _currentSpeed = root.Q<Label>("CurrentSpeed");
        _stock = root.Q<Label>("Stock");
        _ammoCounter = root.Q<VisualElement>("AmmoCounter");
        _speedCounter = root.Q<VisualElement>("SpeedCounter");
        _botBar = root.Q<VisualElement>("BotBar");
        _skullHead = root.Q<VisualElement>("SkullHead");
        _deathMenu = root.Q<VisualElement>("GameOverMenu");
        _retryButton = root.Q<VisualElement>("RetryButton");

        UpdateStock();
    }

    private void RetryGame(ClickEvent evt)
    {
        retryEvent.Invoke();
    }

    public void HideBotBar()
    {
        _botBar.ToggleInClassList("bottom-hidden");
        _skullHead.ToggleInClassList("skull-hidden");
        _deathMenu.ToggleInClassList("death-hidden");
        _retryButton.RegisterCallback<ClickEvent>(RetryGame);
    }

    public void UpdateStock()
    {
        if (player.backpack.weapon is not FiringWeapon fw) return;
        _stock.text = player.backpack.StockAmount(fw.stockType).ToString();
    }

    private void ToggleAmmo(bool active)
    {
        _ammoCounter.style.display = active ? 
            new StyleEnum<DisplayStyle>(DisplayStyle.Flex) : new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    private void ToggleSpeed(bool active)
    {
        _speedCounter.style.display = active ? 
            new StyleEnum<DisplayStyle>(DisplayStyle.Flex) : new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    private void Update()
    {
        _healthBar.value = player.GetHealth();
        _healthBar.highValue = player.maxHealth;
        // _healthBar.style.width = new StyleLength(Length.Percent((float)player.maxHealth / maxHealth * 100));

        _currentHealth.text = player.GetHealth().ToString();
        _maxHealth.text = player.maxHealth.ToString();

        if (player.backpack.weapon is not FiringWeapon fw)
        {
            ToggleAmmo(false);
        }
        else
        {
            ToggleAmmo(true);
            _currentAmmo.text = fw.Ammo.ToString();
        }

        if (player.Speed < player.baseSpeed - 0.1 || player.Speed > player.baseSpeed + 0.1)
        {
            ToggleSpeed(true);
            _currentSpeed.text = Math.Round(player.Speed / player.baseSpeed, 2).ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            ToggleSpeed(false);
        }
        
    }
}
