using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager :SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    public int[] selectedInventoryItem;
    public List<InventoryItem>[] inventoryLists;  //We need several lists like player,chest and so on

    [HideInInspector] public int[] inventoryListsCapacity;

    [SerializeField]
    private SO_ItemList itemList = null;

    protected override void Awake()
    {
        base.Awake();  //重写了父类的Awake但是任然想要其执行作用

        CreatItemDetailsDictionary(); //后面start也会用到这个方法所以最好在这里用Awake

        CreateInventoryLists();

        InitializeSelectedInventoryItemArray();
    }

    

    private void CreatItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();
        foreach(ItemDetails itemDetails in  itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else
        {
            return null;
        }
    }

    public int GetSelectedItemCode(InventoryLocation inventoryLocation)
    {
        return selectedInventoryItem[(int) inventoryLocation];
    }

    public ItemDetails GetSelectedItemDetail(InventoryLocation inventoryLocation)
    {
        int itemCode = selectedInventoryItem[(int)inventoryLocation];

        if(itemCode != -1)
        {
            return GetItemDetails(itemCode);
        }
        else
        {
            return null;
        }
    }

    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int) InventoryLocation.count];

        for(int i=0;i<inventoryLists.Length;i++) 
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListsCapacity = new int[(int)InventoryLocation.count];

        inventoryListsCapacity[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    public void AddInventoryItem(InventoryLocation inventoryLocation,Item item) 
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInventory(inventoryLocation, itemCode);

        if(itemPosition != -1) 
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }

        EventsHandler.InventoryEventsHandler.CallInventoryUpdateEvent(inventoryLocation, inventoryList);
    }
    public void RemoveInventoryItem(InventoryLocation inventoryLocation, Item item)
    {
        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInventory(inventoryLocation, itemCode);

        if(itemPosition != -1)
        {
            RemoveInventoryItemAtPosition(inventoryList, itemCode,itemPosition);
        }

        EventsHandler.InventoryEventsHandler.CallInventoryUpdateEvent(inventoryLocation, inventoryList);
    }

    private void RemoveInventoryItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int itemPosition)
    { 
        InventoryItem newInventoryItem = inventoryList[itemPosition];

        newInventoryItem.itemCode = itemCode;
        newInventoryItem.itemQuantity = inventoryList[itemPosition].itemQuantity - 1;

        if(newInventoryItem.itemQuantity > 0)
        {
            inventoryList[itemPosition] = newInventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(itemPosition);
        }
    }

    public void AddInventoryItem(InventoryLocation inventoryLocation, Item item,GameObject gameObjectDelete)
    {
        AddInventoryItem(inventoryLocation, item);

        Destroy(gameObjectDelete);
    }

    public int FindItemInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for(int i = 0; i < inventoryList.Count; ++i)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        
        }
        return -1;
    }
    
    private void AddItemAtPosition(List<InventoryItem> inventoryList,int itemCode) 
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;

        inventoryList.Add(inventoryItem);

        //DebugInventoryList(inventoryList);
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode,int itemPosition)
    {
    
        InventoryItem newInventoryItem = inventoryList[itemPosition];

        newInventoryItem.itemCode = itemCode;
        newInventoryItem.itemQuantity = inventoryList[itemPosition].itemQuantity+1;

        inventoryList[itemPosition] = newInventoryItem;

        //DebugInventoryList(inventoryList);
    }

    private void DebugInventoryList(List<InventoryItem> inventoryList)
    {
        Debug.Log("******************************************************************");
        Debug.Log("You have:");
        for (int i = 0; i < inventoryList.Count; ++i)
        {
            String ItemDescription = InventoryManager.Instance.GetItemDetails(inventoryList[i].itemCode).itemDescription;
            Debug.Log(ItemDescription+" Quantity:"+ inventoryList[i].itemQuantity);
        }
        Debug.Log("******************************************************************");
    }

    public void SwapInventoryItem(InventoryLocation inventoryLocation,int fromSlotNumber,int toSlotNumber)
    {
        if(fromSlotNumber < inventoryLists[(int)inventoryLocation].Count &&
           toSlotNumber < inventoryLists[(int)inventoryLocation].Count &&
           fromSlotNumber >=0 && toSlotNumber >= 0)
        {

            InventoryItem fromItem = inventoryLists[(int)inventoryLocation][fromSlotNumber];
            InventoryItem toItem = inventoryLists[(int)inventoryLocation][toSlotNumber];
            
            inventoryLists[(int)inventoryLocation][fromSlotNumber] = toItem;
            inventoryLists[(int)inventoryLocation][toSlotNumber] = fromItem;

            EventsHandler.InventoryEventsHandler.CallInventoryUpdateEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }

    private void InitializeSelectedInventoryItemArray()
    {
        selectedInventoryItem = new int[(int)InventoryLocation.count];

        for(int i = 0; i < selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = -1;
        }
    }

    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation,int itemCode)
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode;
    }

    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedInventoryItem[(int)inventoryLocation] = -1;
    }
}
