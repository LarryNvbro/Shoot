using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
[System.Serializable]
public class CameraResolution : MonoBehaviour
{
	public const float AspectScreenRate = 1.1833f; // (960 / 1136)
	public enum CameraMode
	{
		FixedWidth
	}

	Camera myCamera;
	public CameraMode mode = CameraMode.FixedWidth;

    public float devAspect;
    public float devOrthographic;

    //[System.NonSerialized]
	public float currentAspect;
    [System.NonSerialized]
	public static float targetOrthographic;

    [System.NonSerialized]
    public int lastScreenWidth;
    [System.NonSerialized]
    public int lastScreenHeight;
    //[System.NonSerialized]
    //public bool changedScreen;



    public const int DevWidth = 640;    // 개발 기준 해상도
    public const int DevHeight = 1136;  // 개발 기준 해상도

    /// <summary>
    /// 개발 기준 대비 디바이스 Screen 비율
    /// </summary>
    public static float GetAspectScreen()
	{
		float screenAspect = (float)Screen.width / Screen.height; // iPhone5 0.56338028169014084507042253521127 
		float devAspect = (float)DevWidth / DevHeight; // iPhone4 0.6666666666, iPhone5 0.56338

		float resultAspect = devAspect / screenAspect; // 1.183333
		if (resultAspect < 1.0f) resultAspect = 1.0f;
		return resultAspect; // 1.183333
	}
    
	void Awake()
	{
        myCamera = GetComponentInChildren<Camera>();
		
		devAspect = (float)DevWidth / DevHeight;
		devOrthographic = (DevHeight * 0.5f) / 100;

        lastScreenWidth = 0;
        lastScreenHeight = 0;
	}

    void Start()
    {
        //changedScreen = true;

		SetResolutionCamera();

		// 안드로이드 소프트 키보드 변경으로 체크
#if UNITY_EDITOR || UNITY_ANDROID
        StartCoroutine(CheckScreenSize());
#endif
        //Debug.Log(string.Format("CameraResolution: lastWidth {0}, lastHeight {1}, ScreenWidth {2}, ScreenHeight", lastScreenWidth, lastScreenHeight, Screen.width, Screen.height));
    }

	public void SetResolutionCamera()
	{
		currentAspect = (float)Screen.width / (float)Screen.height;

		float rateDevAspect = devAspect / currentAspect;
		targetOrthographic = devOrthographic * rateDevAspect;
		
		// full Screen
 		/*myCamera.orthographicSize = devOrthographic;
 		myCamera.aspect = currentAspect * rateDevAspect;
		currentAspect = myCamera.aspect;
 		myCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);*/

		//*
		if (targetOrthographic < devOrthographic)
		{
			targetOrthographic = devOrthographic;

			// camera clip
			float viewport_margin = (1.0f - rateDevAspect);
			myCamera.rect = new Rect(viewport_margin * 0.5f, 0.0f, rateDevAspect, 1.0f);
		}else
		{
			targetOrthographic = devOrthographic;
			myCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
		}
		
		if (Mathf.Approximately(myCamera.orthographicSize, targetOrthographic) == false) {
			myCamera.orthographicSize = targetOrthographic;
		}
		//*/
	}

// #if UNITY_EDITOR
// 	void Update()
// 	{
//         if (changedScreen) {
//             changedScreen = false;
//             SetResolutionCamera();
//         }
// 	}
// #endif


    IEnumerator CheckScreenSize()
    {
        while (true) 
		{
            if ((lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)) {
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
                SetResolutionCamera();
            }
			
			yield return new WaitForSeconds(0.1f);
        }
    }
}