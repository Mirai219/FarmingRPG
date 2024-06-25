using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInventoryTextbox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextTop1 = null;
    [SerializeField] private TextMeshProUGUI TextTop2 = null;
    [SerializeField] private TextMeshProUGUI TextTop3 = null;
    [SerializeField] private TextMeshProUGUI TextBottom1 = null;
    [SerializeField] private TextMeshProUGUI TextBottom2 = null;
    [SerializeField] private TextMeshProUGUI TextBottom3 = null;

    public void SetTextboxText(string textTop1,string textTop2, string textTop3,string textBottom1,string textBottom2,string textBottom3)
    {
        TextTop1.text = textTop1;
        TextTop2.text = textTop2;
        TextTop3.text = textTop3;
        TextBottom1.text = textBottom1;
        TextBottom2.text = textBottom2;
        TextBottom3.text = textBottom3;
    }
}
