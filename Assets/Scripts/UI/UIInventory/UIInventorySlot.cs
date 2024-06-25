using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIInventorySlot: MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private Camera mainCamera;
    private Transform parentItem;
    private GameObject draggedObject;
    private Canvas parentCanvas;
    private GridCursor gridCursor;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI TextMeshProUGUI;
    [HideInInspector]public bool isSelected = false;

    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private int SlotNumber;
    [SerializeField] private GameObject InventoryTextboxPrefab = null;

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;


    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }
    private void OnEnable()
    {
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent += SceneLoaded;
        //在sceneloaded之后再寻找item，因为start里面不能保证场景一定已经加载好了

    }

    private void OnDisable()
    {
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent -= SceneLoaded;
    }
    private void Start()
    {
        mainCamera = Camera.main;
        gridCursor = FindObjectOfType<GridCursor>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            Player.Instance.DisablePlayerInputAndResetMovement();

            draggedObject = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            Image draggedItemImage = draggedObject.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            draggedObject.transform.position = Input.mousePosition;
        }

        SetSelectedItem();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            Destroy(draggedObject);

            if(eventData.pointerCurrentRaycast.gameObject != null && 
                eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {

                
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().SlotNumber;
            
                
                InventoryManager.Instance.SwapInventoryItem(InventoryLocation.player,SlotNumber,toSlotNumber);

                DestroyInventoryTextbox();

                ClearSelectedItem();
            }
            else
            {
                if(itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }

            Player.Instance.EnablePlayerInput();
        }
    }
    private void DropSelectedItemAtMousePosition()
    {
        if(itemDetails!=null && isSelected)
        {  
            if(gridCursor.CursorPositionIsValid) 
            {
                Vector3 offset = new Vector3(0, -0.5f, 0);
                Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition) + offset;
                GameObject CreatingObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
                Item item = CreatingObject.GetComponent<Item>();

                item.ItemCode = itemDetails.itemCode;


                InventoryManager.Instance.RemoveInventoryItem(InventoryLocation.player, item);


                if (InventoryManager.Instance.FindItemInventory(InventoryLocation.player, item.ItemCode) == -1)
                {
                    ClearSelectedItem();
                }
            } 
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemQuantity != 0)
        {
            inventoryBar.inventoryTextboxGameObject = Instantiate(InventoryTextboxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextboxGameObject.transform.SetParent(parentCanvas.transform,false);

            UIInventoryTextbox inventoryTextbox = inventoryBar.inventoryTextboxGameObject.GetComponent<UIInventoryTextbox>();

            inventoryTextbox.SetTextboxText(itemDetails.itemDescription,"","","","", itemDetails.itemLongDescription);

            if (inventoryBar.InventoryBarBottom)
            {
                inventoryBar.inventoryTextboxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextboxGameObject.transform.position 
                    = new Vector3(transform.position.x,transform.position.y+50f,transform.position.z);

            }
            else
            {
                inventoryBar.inventoryTextboxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextboxGameObject.transform.position
                    = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextbox();
    }

    public void DestroyInventoryTextbox()
    {
        if(inventoryBar.inventoryTextboxGameObject != null) 
        { 
            Destroy(inventoryBar.inventoryTextboxGameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (isSelected)
            {
                ClearSelectedItem();
            }
            else
            {
                if(itemQuantity > 0)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    private void ClearSelectedItem()
    {
        ClearCursor();

        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = false;

        InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);

        Player.Instance.ClearCarriedItem();
    }

    private void SetSelectedItem()
    {
        inventoryBar.ClearHighlightOnInventorySlots();

        isSelected = true;

        inventoryBar.SetHighlightOnInventorySlots();

        gridCursor.ItemUseGridRadius = itemDetails.itemUseGridRadius;

        if(itemDetails.itemUseGridRadius >0)
        {
            gridCursor.EnableCursor();
        }
        else
        {
            gridCursor.DisableCursor();
        }

        gridCursor.ItemType = itemDetails.itemType;

        InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player,itemDetails.itemCode);

        if(itemDetails.canBeCarried)
        {
            Player.Instance.ShowCarriedItem(itemDetails.itemCode);
        }
        else
        {
            Player.Instance.ClearCarriedItem();
        }
    }

    private void SceneLoaded()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
    }

    private void ClearCursor()
    {
        gridCursor.DisableCursor();
        gridCursor.ItemType = ItemType.none;
    }
}
