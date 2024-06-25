using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SO_ItemList", menuName = "Assets/ScriptableObjectAssets/Item")]
public class SO_ItemList :ScriptableObject
{
    //�����л���ʹ��inspector�п��Կ���
    [SerializeField]
    public List<ItemDetails> itemDetails;
}
