using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public PickableItem item;
    private Image icon;
    
    public InventorySlot()
    {
        icon = new Image();
        Add(icon);
        icon.AddToClassList("slotIcon");
        AddToClassList("slot");
    }
}
