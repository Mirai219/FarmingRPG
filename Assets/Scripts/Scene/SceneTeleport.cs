using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour
{
    [SerializeField] private SceneName sceneNameGoTo = SceneName.Scene1_Farm;
    [SerializeField] private Vector3 scenePositiinGoto = new Vector3(); 

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if(player != null)
        {
            float xPosition = Mathf.Approximately(scenePositiinGoto.x, 0f) ? player.transform.position.x : scenePositiinGoto.x;

            float yPosition = Mathf.Approximately(scenePositiinGoto.y, 0f) ? player.transform.position.y : scenePositiinGoto.y;

            float zPosition = 0f;

            SceneControllerManager.Instance.FadeAndLoadScene(sceneNameGoTo.ToString(),
                new Vector3(xPosition, yPosition, zPosition));
        }
    }

}
