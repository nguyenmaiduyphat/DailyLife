using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This is slot where item drag come in
/// </summary>
public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] PlayerInteraction playerInteraction;
    public Transform root;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            Debug.Log("Drop");
         

            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;           
        }
    }

    private bool isMouseOver = false;
    [SerializeField] Color chosenSlot;
    [SerializeField] Color unChosenSlot;

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = chosenSlot;
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = unChosenSlot;
        isMouseOver = false;
    }

    [SerializeField] string nameFolderItem = "Item";
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMouseOver && eventData.button == PointerEventData.InputButton.Right && transform.childCount > 0)
        {
            GameObject[] propList = Resources.LoadAll<GameObject>(nameFolderItem); 
            GameObject propPrefab = propList.FirstOrDefault(x => x.name == transform.GetChild(0).GetComponent<DraggableItem>().itemInfo.namePrefab);
            GameObject prefabSpawned = Instantiate(propPrefab, playerInteraction.transform);
            prefabSpawned.GetComponent<PropInfo>().itemInfo = transform.GetChild(0).GetComponent<DraggableItem>().itemInfo;
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
