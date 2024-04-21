using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ComponentHelper : MonoBehaviour
{
    private const int LayerObjects = 2;

    public static void OnMutatedComponentTestsFailed(ComponentBehaviour c)
    {
    }

    public static void OnComponentDestroyed(ComponentBehaviour c)
    {
        RpgMapHelper.SetAutoTileByPosition(GetPosition(c.gameObject), c.destroyedTileId, LayerObjects);
    }

    public static void OnComponentFixed(ComponentBehaviour c)
    {
        RpgMapHelper.SetAutoTileByPosition(GetPosition(c.gameObject), c.fixedTileId, LayerObjects);
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