using CreativeSpore.RpgMapEditor;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Door : MonoBehaviour
{
    /// <summary> The id of the room which is unlocked by this door in linear progression. </summary>
    [SerializeField]
    protected int roomId;
    
    protected const float OpenDuration = 1f;
    protected const float CloseDuration = 1f;
    
    protected Vector3 InitialPosition;
    protected float InitialScaleY;
    protected bool IsLocked = true;
    protected TweenerCore<Vector3, Vector3, VectorOptions> ScaleTween;
    protected TweenerCore<Vector3, Vector3, VectorOptions> MoveTween;

    private const int BlockLayer = 1;

    private int _initialBlockTileId;
    private InteractableWorldObject _iwo;
    private DoorManager _doorManager;

    public virtual void Start()
    {
        InitialPosition = transform.position;
        InitialScaleY = transform.localScale.y;
        _initialBlockTileId = RpgMapHelper.GetAutoTileByPosition(InitialPosition, BlockLayer).Id;
        _iwo = GetComponent<InteractableWorldObject>();
        _doorManager = FindObjectOfType<DoorManager>();
        _doorManager.Register(roomId, this);
    }
    
    public virtual void Enable()
    {
        _iwo.IsEnabled = true;
    }

    public virtual void Open()
    {
        if (IsLocked) return;
        
        // remove blocking tile
        RpgMapHelper.SetAutoTileByPosition(InitialPosition, 0, BlockLayer);
        
        AbortTweens();
    }

    public virtual void Close()
    {
        if (IsNotClear()) return;
        
        if (IsLocked) RpgMapHelper.SetAutoTileByPosition(InitialPosition, _initialBlockTileId, BlockLayer);
        
        AbortTweens();
    }

    protected bool IsNotClear()
    {
        return _iwo.IsPetInRange || _iwo.IsPlayerInRange;
    }

    public void TryToUnlock()
    {
        _doorManager.TryUnlockRoom(roomId);
    }
    
    public void Unlock()
    {
        IsLocked = false;
        StompEventDelegation.OnRoomUnlocked(roomId);
        Open();
    }

    private void AbortTweens()
    {
        if (ScaleTween != null && ScaleTween.IsActive() && !ScaleTween.IsComplete())
        {
            ScaleTween.Kill();
            ScaleTween = null;
        }

        if (MoveTween != null && MoveTween.IsActive() && !MoveTween.IsComplete())
        {
            MoveTween.Kill();
            MoveTween = null;
        }
    }
}
