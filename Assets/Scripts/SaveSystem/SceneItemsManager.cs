using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : SingletonMonobehaviour<SceneItemsManager>,ISaveable
{
    private Transform parentTransform;
    [SerializeField] private GameObject itemPrefab = null;

    private string _iSaveableUniqueID;

    public string iSaveableUniqueID {  get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value;} }

    private GameObjectSave _gameObjectSave;

    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    public string ISaveableUniqueID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public GameObjectSave gameObjectSave { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void AfterSceneLoad()
    {
        parentTransform = GameObject.FindGameObjectWithTag(Tags.ItemParentTransform).transform;
    }

    protected override void Awake()
    {
        base.Awake();

        iSaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        ISaveableRegister();
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }
    private void OnDisable()
    {
        ISaveableDeregister();
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableStoreScene(string sceneName)
    {
        GameObjectSave.sceneData.Remove(sceneName);

        List<SceneItem> sceneItemList = new List<SceneItem>();
        Item[] itemInScene = GameObject.FindObjectsOfType<Item>();
        
        foreach(Item item in itemInScene)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.itemCode = item.ItemCode;
            sceneItem.position = new Vector3Serializable(item.transform.position.x, item.transform.position.y, item.transform.position.z);
            sceneItem.itemName = item.name;

            sceneItemList.Add(sceneItem);
        }
       
        SceneSave sceneSave = new SceneSave();
        sceneSave.sceneItemList = sceneItemList;

        GameObjectSave.sceneData.Add(sceneName,sceneSave);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if(sceneSave.sceneItemList != null)
            {
                DestroySceneItems();

                InstantiateSceneItems(sceneSave.sceneItemList);
            }
        }
    }
    private void DestroySceneItems()
    {
        Item[] itemInScene = GameObject.FindObjectsOfType<Item>();

        for (int i = 0; i < itemInScene.Length; i++)
        {
            Destroy(itemInScene[i].gameObject);
        }
    }

    private void InstantiateSceneItems(int itemCode,Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, itemPosition, Quaternion.identity, parentTransform);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemCode);
    }

    private void InstantiateSceneItems(List<SceneItem> itemList)
    {
        GameObject itemGameObject;

        foreach (SceneItem sceneItem in itemList)
        {
            itemGameObject = Instantiate(itemPrefab,new Vector3(sceneItem.position.x, sceneItem.position.y, sceneItem.position.z),Quaternion.identity,parentTransform);

            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = sceneItem.itemCode;
            item.name = sceneItem.itemName;
        }
    }
}
