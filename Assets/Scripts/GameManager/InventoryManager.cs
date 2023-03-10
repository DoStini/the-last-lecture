using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class InventoryManager : MonoBehaviour
{
    private VisualElement root;
    public Backpack backpack;

    private Label _itemsCountLabel;
    private Label _weightLabel;
    private ScrollView _itemsView;
    public VisualTreeAsset itemPrefab;

    public bool Active { get; private set; } = true;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        _itemsCountLabel = root.Q<Label>("Count");
        _weightLabel = root.Q<Label>("Weight");
        _itemsView = root.Q<ScrollView>("ItemsView");

        VisualElement itemsContainer = _itemsView.Q<VisualElement>("unity-content-container");
        itemsContainer.style.flexDirection = FlexDirection.Row;
        itemsContainer.style.flexWrap = Wrap.Wrap;

        Toggle();
    }

    public void AddItem(PickableItem item)
    {
        VisualElement el = itemPrefab.Instantiate();
        el.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(item.icon);
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
    
    private void Update()
    {
        _itemsCountLabel.text = backpack.Count + " Items";
        _weightLabel.text = backpack.Weight + " / " + backpack.maxWeight;
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
