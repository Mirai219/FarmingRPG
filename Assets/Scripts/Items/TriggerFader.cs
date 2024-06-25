using UnityEngine;

public class TriggerFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if(obscuringItemFaders.Length > 0)
        {
            for(int i = 0;i<obscuringItemFaders.Length;i++)
            {
                //Debug.Log(obscuringItemFaders[i].name);
                obscuringItemFaders[i].FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if (obscuringItemFaders.Length > 0)
        {
            for (int i = 0; i < obscuringItemFaders.Length; i++)
            {
                //Debug.Log(obscuringItemFaders[i].name);
                obscuringItemFaders[i].FadeIn();
            }
        }
    }
}
