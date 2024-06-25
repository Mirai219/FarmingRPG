using UnityEngine;
using Cinemachine;
public class Switch : MonoBehaviour
{
  
    private void OnEnable()
    {
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent += SwitchConfineBoundingShape;
    }
    private void OnDisable()
    {
        EventsHandler.SceneEventsHandler.AfterSceneLoadEvent -= SwitchConfineBoundingShape;
    }

    private void SwitchConfineBoundingShape()
    {
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();
        
        CinemachineConfiner cinemachineConfiner = gameObject.GetComponent<CinemachineConfiner>();

        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

        //since the confiner bounds have changed need to call this to clear the cache;
        cinemachineConfiner.InvalidatePathCache();
    }


}
