using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    [SerializeField] private Sprite blankSprite = null;
    [SerializeField] private UIInventorySlot[] inventorySlots = null;

    public GameObject inventoryBarDraggedItem;

    private RectTransform rectTransform;

    private bool _inventoryBarBottom =true;

    public bool InventoryBarBottom { get => _inventoryBarBottom; set => _inventoryBarBottom=value; }
    [HideInInspector] public GameObject inventoryTextboxGameObject;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        EventsHandler.InventoryEventsHandler.InventoryUpdateEvent += InventoryUpdate;
    }

    private void Update()
    {
        SwitchInventoryBarPosition();
    }
    private void OnDisable()
    {
        EventsHandler.InventoryEventsHandler.InventoryUpdateEvent -= InventoryUpdate;
    }

    private void SwitchInventoryBarPosition()
    {
        Vector3 playerViewportPosition  = Player.Instance.GetPlayerViewportPosition();

        if(playerViewportPosition.y > 0.3f && !InventoryBarBottom)
        {
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 2.5f);

            InventoryBarBottom = true;
        }

        if(playerViewportPosition.y <= 0.3f && InventoryBarBottom) 
        {
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -2.5f);

            InventoryBarBottom = false;
        }
    }
    private void ClearInventorySlots()
    {
        if(inventorySlots.Length > 0)
        {
            for(int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].inventorySlotImage.sprite = blankSprite;
                inventorySlots[i].TextMeshProUGUI.text = "";
                inventorySlots[i].itemDetails = null;
                inventorySlots[i].itemQuantity = 0;

                SetHighlightOnInventorySlot(i);
            }
        }
    }
    private void InventoryUpdate(InventoryLocation location, List<InventoryItem> list)
    {
        if (location == InventoryLocation.player)
        {
            ClearInventorySlots();

            if (inventorySlots.Length > 0 && list.Count > 0)
            {
                for(int i = 0; i < inventorySlots.Length; i++)
                {
                    if(i < list.Count)
                    {
                        int itemCode = list[i].itemCode;

                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if(itemDetails != null)
                        {
                            inventorySlots[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlots[i].TextMeshProUGUI.text = list[i].itemQuantity.ToString();
                            inventorySlots[i].itemDetails = itemDetails;
                            inventorySlots[i].itemQuantity = list[i].itemQuantity;
                            SetHighlightOnInventorySlot(i);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void ClearHighlightOnInventorySlots()
    {
        if(inventorySlots.Length > 0)
        {
            for(int i = 0;i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].isSelected)
                {
                    inventorySlots[i].isSelected = false;
                    inventorySlots[i].inventorySlotHighlight.color = new Color(0f,0f,0f,0f);

                    InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
                }
            }
        }
    }

    public void SetHighlightOnInventorySlots()
    {
        if( inventorySlots.Length > 0)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                SetHighlightOnInventorySlot(i);
            }
        }
    }

    public void SetHighlightOnInventorySlot(int position)
    {
        if (inventorySlots.Length > 0)
        {          
            if (inventorySlots[position].itemDetails != null && inventorySlots[position].isSelected)
            {
                inventorySlots[position].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f);

                InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player,
                    inventorySlots[position].itemDetails.itemCode);
            }           
        }
    }
}
