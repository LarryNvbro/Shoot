using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVEventListener : MonoBehaviour {

    public System.Action onClick;
    public System.Action<bool> onPress;
    public System.Action<Vector2> onDrag;
    public System.Action<int> onClickMsg;
    private int m_Msg;

    public static NVEventListener Get(GameObject go)
    {
        NVEventListener listener = go.GetComponent<NVEventListener>();
        if(listener == null)
        {
            listener = go.AddComponent<NVEventListener>();
        }
        return listener;
    }

    public NVEventListener SetMsg(int msg)
    {
        m_Msg = msg;
        return this;
    }

    public void OnClick()
    {
        if (onClick != null) onClick();
        if (onClickMsg != null) onClickMsg(m_Msg);
    }

    public void OnPress(bool pressed)
    {
        if (onPress != null)
            if(pressed)
                onPress(pressed);
    }

    public void OnDrag(Vector2 delta)
    {
        if (onDrag != null) onDrag(delta);
            
    }

}
