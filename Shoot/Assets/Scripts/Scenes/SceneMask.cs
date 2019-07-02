using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMask : MonoBehaviour
{
    public GameObject mask;
    private float m_AniDuration = 1.0f;

    public void Open(float interval, System.Action callback)
    {
        Vector2 pos = AppManager.Instance.GetLastTouchPos();
        mask.transform.localPosition = pos;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(interval)
            .Append(mask.transform.DOScale(10, m_AniDuration))
            .AppendCallback(() => callback());
    }

    public void Close(float interval, System.Action callback)
    {
        Vector2 pos = AppManager.Instance.GetLastTouchPos();
        mask.transform.localPosition = pos;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(interval)
            .Append(mask.transform.DOScale(0.0f, m_AniDuration))
            .AppendCallback(() => callback());
    }
}
