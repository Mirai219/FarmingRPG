using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
public class ItemCodeDescriptionDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //chage the return property height to be double to cater for the additional item code description that we will draw
        return EditorGUI.GetPropertyHeight(property)*2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label,property);

        if(property.propertyType == SerializedPropertyType.Integer) 
        { 
            EditorGUI.BeginChangeCheck(); //start of check for changed values

            var newValue = EditorGUI.IntField(new Rect(position.x,position.y,position.width,
                position.height/2),label,property.intValue);

            //draw item desciption
            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, 
                position.width, position.height / 2), "Item Description", 
                GetItemDescription(property.intValue));

            //if item code value has changed,then set value to new value
            if(EditorGUI.EndChangeCheck() )
            {
                property.intValue = newValue;
            }
        }
        EditorGUI.EndProperty();
    }

    private String GetItemDescription(int itemCode)
    {
        SO_ItemList so_ItemList;

        so_ItemList = AssetDatabase.LoadAssetAtPath(
            "Assets/ScriptableObjectAssets/Item/SO_ItemList.asset",
            typeof(SO_ItemList)) as SO_ItemList;

        List<ItemDetails> itemDetailsList = so_ItemList.itemDetails;

        ItemDetails itemDetails  = itemDetailsList.Find(x => x.itemCode ==  itemCode);

        if(itemDetails != null)
        {
            return itemDetails.itemDescription;
        }
        else
        {
            return "";
        }
    }   

}
