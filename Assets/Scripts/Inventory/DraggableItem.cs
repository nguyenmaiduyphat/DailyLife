using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class for dragging item in inventory
/// </summary>
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public InventorySlot inventorySlot;

    [HideInInspector] public Image image;
    private Transform root;
    public ItemInfo itemInfo;

    private void OnEnable()
    {
        inventorySlot = GetComponentInParent<InventorySlot>();
        root = GetComponentInParent<InventorySlot>().root;
        image = GetComponent<Image>();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }


    
}
