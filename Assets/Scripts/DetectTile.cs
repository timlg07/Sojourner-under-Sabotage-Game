using System.Collections;
using System.Collections.Generic;
using CreativeSpore.RpgMapEditor;
using UnityEngine;

public class DetectTile : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        Debug.Log("transform.position = " + transform.position);
        for (int layer = 0; layer < 3; layer++)
        {
            var tile = RpgMapHelper.GetAutoTileByPosition(transform.position, layer);
            Debug.Log("layer["+layer+"].id = " + tile.Id);
        }
#endif
    }
}