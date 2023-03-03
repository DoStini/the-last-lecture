using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Player player;

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

        UpdateStock();
    }

    public void UpdateStock()
    {
        if (ReferenceEquals(player.weapon, null) || player.weapon is not FiringWeapon fw) return;
        _stock.text = player.backpack.StockAmount(fw.stockType).ToString();
    }

    private void ToggleAmmo(bool active)
    {
        _ammoCounter.style.display = active ? 
            new StyleEnum<DisplayStyle>(DisplayStyle.Flex) : new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    private void Update()
    {
        _healthBar.value = player.GetHealth();
        _healthBar.highValue = player.maxHealth;

        if (ReferenceEquals(player.weapon, null) || player.weapon is not FiringWeapon fw)
        {
            ToggleAmmo(false);
            return;
        }
        ToggleAmmo(true);

        _currentAmmo.text = fw.Ammo.ToString();
    }
}
