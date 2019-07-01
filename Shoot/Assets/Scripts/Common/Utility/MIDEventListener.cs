using UnityEngine;
using System.Collections;

public class MIDEventListener : MonoBehaviour 
{
	public System.Action onClick;
	public System.Action<bool> onPress;
	public System.Action<Vector2> onDrag;
    public System.Action<int> onClickMsg;
    private int m_Msg;

	public static MIDEventListener Get(GameObject go)
	{
        MIDEventListener listener = go.GetComponent<MIDEventListener>();
		if(listener == null)
		{
			listener = go.AddComponent<MIDEventListener>();
		}
		return listener;
	}

    public MIDEventListener SetMsg(int msg)
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
		if (onPress != null) onPress(pressed);
	}

	
	public void OnDrag(Vector2 delta) 
	{
		if (onDrag != null) onDrag(delta); 
	}
	
}
