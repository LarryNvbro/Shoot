using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraExtension : MonoBehaviour
{
    private static Camera _camera;
    private bool isDritry;

    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float m_Zoom = 1.0f;

    [SerializeField]
    [Beebyte.Obfuscator.SkipRename]
    public float Zoom
    {
        get
        {
            return m_Zoom;
        }
        set
        {
            if (m_Zoom != value)
            {
                m_Zoom = value;
                isDritry = true;
            }
        }
    }
    private float m_EndZoom;

    [SerializeField]
    private Vector3 m_ZoomPosition;
    [Beebyte.Obfuscator.SkipRename]
    public Vector3 ZoomPosition
    {
        get { return m_ZoomPosition; }
        set
        {
            if (m_ZoomPosition != value)
            {
                m_ZoomPosition = value;
                isDritry = true;
            }
        }
    }

    [SerializeField]
    private Vector3 m_ShakePosition;
    [Beebyte.Obfuscator.SkipRename]
    public Vector3 ShakePosition
    {
        get
        {
            return m_ShakePosition;
        }
        set
        {
            if (m_ShakePosition != value)
            {
                m_ShakePosition = value;
                isDritry = true;
            }
        }
    }

    [Range(0.0f, 10.0f)]
    public float shakeTestTime;

    [SerializeField, Candlelight.PropertyBackingField("ShakeTest")]
    private bool m_ShakeTest;
    [Beebyte.Obfuscator.SkipRename]
    public bool ShakeTest
    {
        set { Shake(shakeTestTime); }
    }

    private static CameraExtension _instance;
    public bool m_Shake = false;

    private void Awake()
    {
        _instance = this;
        _camera = GetComponentInChildren<Camera>();
        m_Zoom = 1.0f;
        m_ZoomPosition = Vector3.zero;
        isDritry = false;
    }

    private void OnDestroy()
    {
        _instance = null;
        _camera = null;
    }

    void LateUpdate()
    {
        if (m_Shake)
        {
            m_Shake = false;
            Shake();
        }
        ComputeCamera();
    }

    void ComputeCamera()
    {
        if (isDritry)
        {
            _instance.transform.position = m_ShakePosition + m_ZoomPosition;
            isDritry = false;
        }
    }

    public static void Shake(float duration = 0.5f, float shakeAmplitude = 0.1f, int vibrato = 15)
    {
        if (_instance != null)
        {
            if (DOTween.IsTweening("camera_shake"))
            {
                List<Tween> tweens = DOTween.TweensById("camera_shake", false);
                for (int i = 0; i < tweens.Count; ++i)
                    tweens[i].Complete();
            }
            _instance.m_ShakePosition = Vector3.zero;
            var tween = DOTween.Shake(() => _instance.ShakePosition, x => _instance.ShakePosition = x, duration, shakeAmplitude, vibrato, 90, false);
            tween.id = "camera_shake";
        }
    }
}
