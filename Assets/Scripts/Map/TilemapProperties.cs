using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapProperties : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private SO_GridProperties gridProperties = null;
    [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;

    private void OnEnable()
    {
        if (!Application.IsPlaying(gameObject))
        {
            tilemap = GetComponent<Tilemap>();

            if(gridProperties != null)
            {
                gridProperties.gridPropertiesList.Clear();
            }
        }
    }

    private void OnDisable()
    {
        if(!Application.IsPlaying(gameObject))
        {
            UpdateGridProperties();

            if(gridProperties != null)
            {
                EditorUtility.SetDirty(gridProperties);
            }
        }
    }

    private void UpdateGridProperties()
    {
        tilemap.CompressBounds();

        if(!Application.IsPlaying(gameObject) )
        {
            if(gridProperties != null)
            {
                Vector3Int startCell = tilemap.cellBounds.min;
                Vector3Int endCell = tilemap.cellBounds.max;

                for(int x = startCell.x; x < endCell.x; ++x)
                {
                    for(int y = startCell.y; y < endCell.y; ++y)
                    {
                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y,0));

                        if(tile != null)
                        {
                            gridProperties.gridPropertiesList.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperty, true));
                        }   
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {

            Debug.Log("Disable property tilemap");
        }

    }
}
