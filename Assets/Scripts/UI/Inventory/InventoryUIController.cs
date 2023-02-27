using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private int slots;
    private List<InventorySlot> InventoryItems = new();
    private VisualElement m_Root;
    private VisualElement m_SlotContainer;
    private void Start()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_SlotContainer = m_Root.Query("SlotsContainer");

        for (int i = 0; i < slots; i++)
        {
            InventorySlot item = new InventorySlot();
            InventoryItems.Add(item);
            m_SlotContainer.Add(item);
        }
    }
}
