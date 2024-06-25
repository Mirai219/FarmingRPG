using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGUID))]
public class GridPropertyManager : SingletonMonobehaviour<GridPropertyManager> ,ISaveable
{
    public Grid grid;
    private Dictionary<string, GridPropertyDetail> gridPropertyDictionary ;
    [SerializeField] private SO_GridProperties[] so_gridPropetyArray;


    //下面两个字段都是ISaveable所要求有的
    private string _iSaveableUniqueID;
    public string ISaveableUniqueID {  get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value;} }

    public GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave {  get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    public GameObjectSave gameObjectSave { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected override void Awake()
    {
        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        ISaveableRegister();
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent += AfterScenenLoad;
    }

    private void Start()
    {
        InitializeGridProperties();
    }

    private void AfterScenenLoad()
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }

    private void OnDisable()
    {
        ISaveableDeregister();
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent += AfterScenenLoad;
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

        SceneSave sceneSave = new SceneSave();

        sceneSave.GridPropertyDetailDictionary = gridPropertyDictionary;

        GameObjectSave.sceneData.Add(sceneName, sceneSave);

    }

    public void ISaveableRestoreScene(string sceneName)
    {
        if(GameObjectSave.sceneData.TryGetValue(sceneName,out SceneSave sceneSave))
        {
            if(sceneSave.GridPropertyDetailDictionary != null)
            {
                gridPropertyDictionary = sceneSave.GridPropertyDetailDictionary;
            }
        }
    }

    private void InitializeGridProperties()
    {
        foreach(SO_GridProperties so_gridProperties in so_gridPropetyArray)
        {
            Dictionary<string,GridPropertyDetail> gridPropertyDetailDictionary = new Dictionary<string,GridPropertyDetail>();

            foreach(GridProperty gridProperty in so_gridProperties.gridPropertiesList)
            {
                GridPropertyDetail gridPropertyDetail;

                gridPropertyDetail = GetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDetailDictionary);

                if(gridPropertyDetail == null)
                {
                    gridPropertyDetail = new GridPropertyDetail();
                }

                switch(gridProperty.gridBoolProperty) 
                {
                    case GridBoolProperty.canDropItem:
                        gridPropertyDetail.canDropItem = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.diggable:
                        gridPropertyDetail.isDiggable = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.isNPCObstacle:
                        gridPropertyDetail.isNPCObstacle = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.canPlaceFurniture:
                        gridPropertyDetail.canPlaceFurniture = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.isPath:
                        gridPropertyDetail.isPath = gridProperty.gridBoolValue;
                        break;
                    default:
                        break;
                }

                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y,gridPropertyDetail, gridPropertyDetailDictionary);
            }

            SceneSave sceneSave = new SceneSave();

            sceneSave.GridPropertyDetailDictionary = gridPropertyDetailDictionary;

            if(so_gridProperties.sceneName.ToString() == SceneControllerManager.Instance.startingSceneName.ToString())
            {
                this.gridPropertyDictionary = gridPropertyDetailDictionary;
            }

            GameObjectSave.sceneData.Add(so_gridProperties.sceneName.ToString(), sceneSave);
        }
    }

    private void SetGridPropertyDetails(int x, int y, GridPropertyDetail gridPropertyDetail, Dictionary<string, GridPropertyDetail> gridPropertyDictionary)
    {
        string key = "x" + x + "y" + y;

        gridPropertyDetail.gridX = x;
        gridPropertyDetail.gridY = y;

        gridPropertyDictionary[key] = gridPropertyDetail;
    }

    public GridPropertyDetail GetGridPropertyDetails(int x, int y, Dictionary<string, GridPropertyDetail> gridPropertyDictionary)
    {
        if(gridPropertyDictionary != null) 
        { 
            string key = "x" + x + "y" + y;

            GridPropertyDetail gridPropertyDetail;

            if(!gridPropertyDictionary.TryGetValue(key, out gridPropertyDetail))
            {
                return null;
            }
            else
            {
                return gridPropertyDetail;
            }
        }
        else
        {
            return null;
        }
    }

    public GridPropertyDetail GetGridPropertyDetail(int x,int y)
    {
        string key = "x" + x + "y" + y;
        GridPropertyDetail gridPropertyDetail;
        
        if(gridPropertyDictionary.TryGetValue(key,out gridPropertyDetail))
        {
            return gridPropertyDetail;
        }
        else
        {
            return null;
        }
    }
}
