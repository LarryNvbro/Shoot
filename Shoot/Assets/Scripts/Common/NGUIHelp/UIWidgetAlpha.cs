using UnityEngine;

[ExecuteInEditMode]
public class UIWidgetAlpha : MonoBehaviour
{
    public float alpha = 1f;
    public UIWidget widget;

	void OnEnable()
	{
		Update();
	}


    void Update() 
	{ 
		if (widget != null)
		{
			widget.alpha = alpha; 
		}
	}
}
