using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour {

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {            
            return GlobalSettings.Instance.CanControlThisPlayer(playerOwner);
        }
    }

    protected virtual Player playerOwner
    {
        get{
            
            if (tag.Contains("Low"))
                return GlobalSettings.Instance.LowPlayer;
            else if (tag.Contains("Top"))
                return GlobalSettings.Instance.TopPlayer;
            else
            {
                Debug.LogError("Untagged Card or creature " + transform.parent.name);
                return null;
            }
        }
    }

    protected abstract bool DragSuccessful();
}
