using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class InventoryManager : MonoBehaviour
{
    private VisualElement root;
    private VisualElement _itemInfo;
    private Label _itemInfoTitle;
    private Label _itemInfoDescription;
    private VisualElement _infoContent;
    private VisualElement _infoSep;


    private bool _infoToggled;
    public Backpack backpack;

    private Label _itemsCountLabel;
    private Label _weightLabel;
    private ScrollView _itemsView;
    public VisualTreeAsset itemPrefab;
    public VisualTreeAsset infoPairPrefab;

    public bool Active { get; private set; } = true;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        _itemsCountLabel = root.Q<Label>("Count");
        _weightLabel = root.Q<Label>("Weight");
        _itemsView = root.Q<ScrollView>("ItemsView");
        _itemInfo = root.Q<VisualElement>("ItemInfo");
        
        _itemInfoTitle = _itemInfo.Q<Label>("InfoTitle");
        _itemInfoDescription = _itemInfo.Q<Label>("InfoDescription");
        _infoContent = _itemInfo.Q<VisualElement>("InfoContent");
        _infoSep = _itemInfo.Q<VisualElement>("InfoSep");
        
        VisualElement itemsContainer = _itemsView.Q<VisualElement>("unity-content-container");
        itemsContainer.style.flexDirection = FlexDirection.Row;
        itemsContainer.style.flexWrap = Wrap.Wrap;

        Toggle();
    }

    public void AddItem(PickableItem item)
    {
        VisualElement el = itemPrefab.Instantiate();
        el.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(item.icon);
        el.Q<VisualElement>("Item").RegisterCallback<MouseEnterEvent>((evt =>
        {
            UpdateItemInfo(item);
            _infoToggled = true;
        }));
        el.Q<VisualElement>("Item").RegisterCallback<MouseLeaveEvent>((evt =>
        {
            _infoToggled = false;
            _itemInfo.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }));
        el.Q<VisualElement>("Drop").AddManipulator(new Clickable(
            () =>
            {
                backpack.RemovePickableItem(item);
            }));

        var actionElement = el.Q<VisualElement>("Action");
        if (item.HasAction())
        {
            actionElement.AddManipulator(new Clickable(item.RunAction));
        }
        else
        {
            actionElement.style.display = DisplayStyle.None;
        }

        el.name = item.GetInstanceID().ToString();

        _itemsView.Add(el);
    }

    public void RemoveItem(PickableItem item)
    {
        VisualElement el = _itemsView.Q<VisualElement>(item.GetInstanceID().ToString());
        el.RemoveFromHierarchy();
    }

    private void UpdateItemInfo(PickableItem item)
    {
        switch (item)
        {
            case Health health:
                UpdateHealth(health);
                break;
            case MaxHealth maxHealth:
                UpdateMaxHealth(maxHealth);
                break;
            case Speed speed:
                UpdateSpeed(speed);
                break;
            case Stock stock:
                UpdateStock(stock);
                break;
        }
    }

    private void UpdateStock(Stock stock)
    {
        switch (stock.type)
        {
            case Stock.Type.Pistol:
                _itemInfoTitle.text = "Pistol Mag";
                _itemInfoDescription.text = "Used to reload a Pistol";
                break;
            case Stock.Type.Rifle:
                _itemInfoTitle.text = "Rifle Mag";
                _itemInfoDescription.text = "Used to reload a Rifle";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateSpeed(Speed speed)
    {
        _itemInfoTitle.text = "Mountain Dew";
        _itemInfoDescription.text = "Increases your speed for a short time";
    }

    private void UpdateMaxHealth(MaxHealth maxHealth)
    {
        _itemInfoTitle.text = "Zombie Vaccine";
        _itemInfoDescription.text = "Increases your max health";
    }

    private void UpdateHealth(Health health)
    {
        _itemInfoTitle.text = "Baguette";
        _itemInfoDescription.text = "Heals you for a certain amount";
    }

    private void Update()
    {
        _itemsCountLabel.text = backpack.Count + " Items";
        _weightLabel.text = backpack.Weight + " / " + backpack.maxWeight;

        if (!_infoToggled) return;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = RuntimePanelUtils.ScreenToPanel(_itemInfo.panel, mousePos);

        if (mousePos.y - _itemInfo.resolvedStyle.height < 0)
        {
            _itemInfo.style.bottom = mousePos.y + 3;
            _itemInfo.style.top = new StyleLength(StyleKeyword.Auto);
        }
        else
        {        
            _itemInfo.style.top = root.resolvedStyle.height - mousePos.y + 3;
            _itemInfo.style.bottom = new StyleLength(StyleKeyword.Auto);
        }
        
        _itemInfo.style.left = mousePos.x + 3;
        _itemInfo.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
    }

    public void Toggle()
    {
        root.style.display = Active ? DisplayStyle.None : DisplayStyle.Flex;
        Active = !Active;
        Cursor.visible = Active;
    }

    public void Disable()
    {
        root.style.display = DisplayStyle.None;
        Active = false;
    }
}
