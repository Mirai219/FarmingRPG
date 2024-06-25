using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
    private Canvas canvas;
    private Grid grid;
    private Camera mainCamera;

    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite redCursorSprite = null;

    private bool _cursorPositionIsValid = false;

    public bool CursorPositionIsValid { get=>_cursorPositionIsValid; set=>_cursorPositionIsValid = value;}

    private int _itemUseGridRadius = 0;

    public int ItemUseGridRadius {  get=>_itemUseGridRadius; set=>_itemUseGridRadius = value;}

    private ItemType _selectedItemType;

    public ItemType ItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private bool _cursorIsEnabled = false;

    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value;}

    private void OnEnable()
    {
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent += AfterSceneLoaded;
    }

    private void OnDisable()
    {
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
    }

    private void AfterSceneLoaded()
    {
        grid = GameObject.FindObjectOfType<Grid>(); 
    }

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if(CursorIsEnabled)
        {
            DisplayCursor();
        }
    }

    private Vector3Int DisplayCursor()
    {
        if(grid != null)
        {
            Vector3Int cursorPosition = GetGridPositionForCursor();

            Vector3Int playerPosition = GetGridPositionForPlayer();

            SetCursorValidity(cursorPosition, playerPosition);

            cursorRectTransform.position = GerRectTransformPositionForCursor(cursorPosition);

            return cursorPosition;
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    private void SetCursorValidity(Vector3Int cursorPosition, Vector3Int playerPosition)
    {
        SetCursorToValid();

        if(Mathf.Abs(cursorPosition.x - playerPosition.x) > ItemUseGridRadius || Mathf.Abs(cursorPosition.y - playerPosition.y) > ItemUseGridRadius)
        {
            SetCursorToInvalid();
            return;
        }

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedItemDetail(InventoryLocation.player);

        if(itemDetails == null)
        {
            SetCursorToInvalid();
            return;
        }

        GridPropertyDetail gridPropertyDetail = GridPropertyManager.Instance.GetGridPropertyDetail(cursorPosition.x, cursorPosition.y);

        if(gridPropertyDetail != null)
        {
            switch(itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (!isCursorValidForSeed(gridPropertyDetail))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.Commodity:
                    if (!isCursorValidForCommodity(gridPropertyDetail))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;

                case ItemType.none:
                    break;

                case ItemType.count:
                    break;

                default: break;
            }
        }
        else
        {
            SetCursorToInvalid();
            return;
        }
    }

    private bool isCursorValidForCommodity(GridPropertyDetail gridPropertyDetail)
    {
        return gridPropertyDetail.canDropItem;
    }

    private bool isCursorValidForSeed(GridPropertyDetail gridPropertyDetail)
    {
        return gridPropertyDetail.canDropItem;
    }

    private void SetCursorToInvalid()
    {
        cursorImage.sprite = redCursorSprite;
        CursorPositionIsValid = false;
    }

    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;
    }

    private Vector3 GerRectTransformPositionForCursor(Vector3Int cursorPosition)
    {
        Vector3 cursorWorldPosition = grid.CellToWorld(cursorPosition);
        Vector2 cursorScreenPosition = mainCamera.WorldToScreenPoint(cursorWorldPosition);
        return RectTransformUtility.PixelAdjustPoint(cursorScreenPosition, cursorRectTransform,canvas);
    }

    private Vector3Int GetGridPositionForPlayer()
    {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    public Vector3Int GetGridPositionForCursor()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));
        return grid.WorldToCell(worldPosition);
    }

    public void DisableCursor()
    {
        CursorIsEnabled = false;
        cursorImage.color = Color.clear;
    }

    public void EnableCursor()
    {
        CursorIsEnabled = true;
        cursorImage.color = new Color(1,1,1,1);
    }
}