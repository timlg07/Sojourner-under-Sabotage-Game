using CreativeSpore.RpgMapEditor;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected const float OpenDuration = 1f;
    protected const float CloseDuration = 1f;
    
    protected Vector3 InitialPosition;
    protected bool IsLocked = true;

    private const int BlockLayer = 1;

    private int _initialBlockTileId;
    private InteractableWorldObject _iwo;

    public virtual void Start()
    {
        InitialPosition = transform.position;
        _initialBlockTileId = RpgMapHelper.GetAutoTileByPosition(InitialPosition, BlockLayer).Id;
        _iwo = GetComponent<InteractableWorldObject>();
    }

    public virtual void Open()
    {
        if (IsLocked) return;
        
        // remove blocking tile
        RpgMapHelper.SetAutoTileByPosition(InitialPosition, 0, BlockLayer);
    }

    public virtual void Close()
    {
        if (IsNotClear()) return;
        if (IsLocked) RpgMapHelper.SetAutoTileByPosition(InitialPosition, _initialBlockTileId, BlockLayer);
    }

    protected bool IsNotClear()
    {
        return _iwo.IsPetInRange || _iwo.IsPlayerInRange;
    }

    public void Unlock()
    {
        IsLocked = false;
        Open();
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        // reset blocking tile
        if (Application.isPlaying && _initialBlockTileId != -1)
        {
            RpgMapHelper.SetAutoTileByPosition(InitialPosition, _initialBlockTileId, BlockLayer);
        }
#endif
    }
}
