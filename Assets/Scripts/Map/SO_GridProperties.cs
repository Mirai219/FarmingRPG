using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GridProperties", menuName = "Assets/ScriptableObjectAssets/GridProperty")]
public class SO_GridProperties : ScriptableObject
{
    public SceneName sceneName;
    public int gridWidth;
    public int gridHeight;
    public int originX;
    public int originY;

    [SerializeField]
    public List<GridProperty> gridPropertiesList;
}
