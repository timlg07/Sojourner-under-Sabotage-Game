using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ComponentHelper : MonoBehaviour
{
    private const int LayerGroundOverlay = 1;
    private const int DestroyedComponentTileId = 262;
    private const int FixedComponentTileId = 263;

    public static void OnMutatedComponentTestsFailed(ComponentBehaviour c)
    {
    }

    public static void OnComponentDestroyed(ComponentBehaviour c)
    {
        RpgMapHelper.SetAutoTileByPosition(GetPosition(c.gameObject), DestroyedComponentTileId, LayerGroundOverlay);
    }

    public static void OnComponentFixed(ComponentBehaviour c)
    {
        RpgMapHelper.SetAutoTileByPosition(GetPosition(c.gameObject), FixedComponentTileId, LayerGroundOverlay);
    }

    public static Vector3 GetPosition(GameObject o)
    {
        return (FindChildWithTag(o, "tilePos") ?? o).transform.position;
    }

    public static GameObject FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform parentTransform in parent.transform)
        {
            if (parentTransform.CompareTag(tag))
            {
                return parentTransform.gameObject;
            }
        }

        return null;
    }
}