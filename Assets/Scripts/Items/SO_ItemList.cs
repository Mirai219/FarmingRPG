using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SO_ItemList", menuName = "Assets/ScriptableObjectAssets/Item")]
public class SO_ItemList :ScriptableObject
{
    //可序列化，使在inspector中可以看到
    [SerializeField]
    public List<ItemDetails> itemDetails;
}
