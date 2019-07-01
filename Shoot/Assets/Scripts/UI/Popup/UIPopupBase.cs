using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UIPopupButtonType
{
    None,
    Ok,
    OkCancel,
}
public enum UIPopupPrefab
{
    Ok,
    OkCancel,
}
public enum UIPopupMsg
{
    Ok,
    Cancel,
}
public enum UIPopupTween
{
    None,
    Tween,
}

public delegate void PopupMessage(UIPopupBase popupBase);

public class UIPopupBase : MonoBehaviour
{
    public const int PopupBaseDepth = 1000;

    public class PopupParameter
    {
        public UIPopupMsg msg;
        public int param;
        public object obj;
        public PopupParameter() { }
    }

    public static List<UIPopupBase> listPopup = new List<UIPopupBase>();
    private static UIPanel cacheRootPanel;
    private static GameObject prefabPopupBase;

    public static int PopupCount { get { return listPopup.Count; } }

    int popDepth;
    public PopupParameter popupParam = new PopupParameter();
    public UIPopupTween popupTween = UIPopupTween.None;
    public int popupDepthBase
    {
        get { return popDepth * PopupBaseDepth; }
    }

    protected GameObject popupRoot;
    UISprite popupBaseBG;
    UIWidget popupTouchBlock;
    [SerializeField]
    protected UILabel popupTitle;
    [SerializeField]
    protected UILabel popupText;
    public PopupMessage onPopupMessage;
    public GameObject okButton;

    private bool isKeyBackUsed = false;
    public bool IsKeyBackUsed
    {
        get { return isKeyBackUsed; }
        set { isKeyBackUsed = value; }
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnDestoryPopup() { }
    public virtual void OnOpenComplete() { }
    public virtual void OnCloseComplete() { }

    private string popupTagName;
    public string PopupTag
    {
        set { popupTagName = value; }
        get { return popupTagName; }
    }

    protected bool isPopupTween = false;
    public bool IsPopupTweening { get { return isPopupTween; } }

    public bool PopupEnable
    {
        set { popupRoot.SetActive(value); }
        get { return popupRoot.activeSelf; }
    }
    public Color BGColor
    {
        set { if (popupBaseBG != null) popupBaseBG.color = value; }
        get { return popupBaseBG == null ? Color.white : popupBaseBG.color; }
    }
    public bool BGEnable
    {
        set { if (popupBaseBG != null) popupBaseBG.enabled = value; }
        get { return popupBaseBG == null ? false : popupBaseBG.enabled; }
    }
    public virtual string PopupText
    {
        set
        {
            if (popupText == null) return;
            popupText.text = value;
        }
        get
        {
            if (popupText == null) return null;
            return popupText.text;
        }
    }
    public virtual string PopupTitle
    {
        set
        {
            if (popupTitle == null) return;
            popupTitle.text = value;
        }
        get
        {
            if (popupTitle == null) return null;
            return popupTitle.text;
        }
    }

    private void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        if (popupTouchBlock != null)
        {
            popupTouchBlock.gameObject.AddComponent<UIButton>().onClick.Add(new EventDelegate(OnPopupCancel));
        }
        OnStart();
        PopupOpen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            listPopup[listPopup.Count - 1].OnPopupCancel();
    }

    public void PopupOpen()
    {
        IsKeyBackUsed = true;
        OnPopupTweenOpen(delegate ()
        {
            IsKeyBackUsed = false;
            OnOpenComplete();
        });
    }

    private void SetTouchBlockLowDepth()
    {
        if (popupTouchBlock != null)
        {
            popupTouchBlock.depth = -PopupBaseDepth;
        }
    }

    public void PopupClose()
    {
        IsKeyBackUsed = true;
        OnPopupTweenClose(delegate ()
        {
            IsKeyBackUsed = false;
            OnCloseComplete();
            OnExecuteMessage();
        });
    }

    public virtual void OnPopupOk()
    {
        if (IsPopupTweening) return;
        if (IsKeyBackUsed) return;
        popupParam.msg = UIPopupMsg.Ok;
        PopupClose();
    }

    public virtual void OnPopupCancel()
    {
        if (IsPopupTweening) return;
        if (IsKeyBackUsed) return;
        popupParam.msg = UIPopupMsg.Cancel;
        PopupClose();
    }

    public virtual void SetButtonType(UIPopupButtonType type)
    {

    }

    protected virtual void OnPopupTweenOpen(System.Action onFinish)
    {
        if (popupTween == UIPopupTween.None)
        {
            if (onFinish != null) onFinish();
            return;
        }
        isPopupTween = true;
        float duration = 0.1f;
        Transform trans = gameObject.transform;
        trans.localScale = new Vector3(0.1f, 0.1f);

        TweenScale tween = TweenScale.Begin(this.gameObject, duration, Vector3.one);
        tween.ignoreTimeScale = true;
        tween.SetOnFinished(delegate ()
        {
            isPopupTween = false;
            trans.localScale = Vector3.one;
            if (onFinish != null) onFinish();
        });
    }

    protected virtual void OnPopupTweenClose(System.Action onFinish)
    {
        if (popupTween == UIPopupTween.None)
        {
            if (onFinish != null) onFinish();
            return;
        }
        isPopupTween = true;
        float duration = 0.1f;
        Transform trans = gameObject.transform;
        trans.localScale = Vector3.one;

        TweenScale tween = TweenScale.Begin(this.gameObject, duration, new Vector3(0.01f, 0.01f));
        tween.ignoreTimeScale = true;
        tween.SetOnFinished(delegate ()
        {
            isPopupTween = false;
            trans.localScale = Vector3.zero;
            if (onFinish != null) onFinish();
        });
    }

    public static UIPopupBase Push(UIPopupPrefab type)
    {
        string prefabs_Popup = "popup_common_ok";
        UIPopupBase popup = Push(prefabs_Popup);

        if (type == UIPopupPrefab.Ok)
            popup.SetButtonType(UIPopupButtonType.Ok);
        else if (type == UIPopupPrefab.OkCancel)
            popup.SetButtonType(UIPopupButtonType.OkCancel);

        return popup;
    }

    public static UIPopupBase Push(string prefabName)
    {
        string prefabPath = string.Format("Prefabs/Popup/{0}", prefabName);
        GameObject prefab = ResourceHelper.LoadPrefab(prefabPath);
        if (prefab == null)
            return null;

        GameObject go = Instantiate(prefab) as GameObject;
        go.name = prefab.name;
        Transform trans = go.transform;
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;

        UIPopupBase popupBase = Push(go);

        return popupBase;
    }

    public static UIPopupBase Push(GameObject popup_obj)
    {
        if (popup_obj == null) return null;
        if (cacheRootPanel == null)
            cacheRootPanel = NGUITools.CreateUI(false, 8);

        if (prefabPopupBase == null)
            prefabPopupBase = ResourceHelper.LoadPrefab("Prefabs/Popup/ui_popup_create") as GameObject;

        GameObject create_root = NGUITools.AddChild(cacheRootPanel.gameObject, prefabPopupBase);

        UIPopupBase ui_popup = popup_obj.GetComponent<UIPopupBase>();
        if (ui_popup == null)
            ui_popup = popup_obj.AddComponent<UIPopupBase>();
        ui_popup.popupTagName = popup_obj.name;
        ui_popup.popupRoot = create_root;
        ui_popup.popupBaseBG = create_root.transform.Find("ui_base_bg").GetComponent<UISprite>();
        ui_popup.popupTouchBlock = create_root.transform.Find("ui_touch_block").GetComponent<UIWidget>();

        int arrCount = listPopup.Count;
        ui_popup.popDepth = 1;
        if (arrCount > 0)
            ui_popup.popDepth = listPopup[arrCount - 1].popDepth + 1;
        int addPopupDepth = ui_popup.popDepth + PopupBaseDepth;
        int basePopupDepth = addPopupDepth;

        UIPanel[] arrPanel = popup_obj.GetComponentsInChildren<UIPanel>(true);
        if (arrPanel.Length > 0)
        {
            for (int i = 0; i < arrPanel.Length; ++i)
            {
                arrPanel[i].depth += addPopupDepth;
                arrPanel[i].sortingOrder = arrPanel[i].depth;
                if (basePopupDepth >= arrPanel[i].depth)
                    basePopupDepth = arrPanel[i].depth - 1;
            }
        }

        UIPanel root_panel = create_root.GetComponent<UIPanel>();
#if UNITY_EDITOR
        create_root.name = "PopupBase " + ui_popup.popDepth;
#endif
        root_panel.depth = basePopupDepth + 1;
        root_panel.sortingOrder = basePopupDepth + 1;
        listPopup.Add(ui_popup);

        Transform popTrans = popup_obj.transform;
        Vector3 position = popTrans.localPosition;
        Quaternion rotation = popTrans.localRotation;
        Vector3 scale = popTrans.localScale;

        popTrans.parent = create_root.transform;
        popTrans.localPosition = position;
        popTrans.localRotation = rotation;
        popTrans.localScale = scale;
        popup_obj.layer = create_root.layer;

        NGUITools.MarkParentAsChanged(create_root);

        return ui_popup;
    }

    public static void Pop(UIPopupBase popup)
    {
        if (popup == null) return;

        int index = listPopup.IndexOf(popup);
        if (index >= 0)
        {
            listPopup.RemoveAt(index);
            popup.PopupEnable = false;
            NGUITools.Destroy(popup.popupRoot);
        }
    }

    public static void Clear()
    {
        for (int i = 0; i < listPopup.Count; ++i)
            NGUITools.Destroy(listPopup[i].popupRoot);
        listPopup.Clear();
    }

    public static void SimpleNotice(string text, string title ="", System.Action action = null)
    {
        UIPopupBase popup = Push("ui_popup_common_ok");
        popup.PopupTitle = title;
        popup.PopupText = text;
        popup.onPopupMessage = delegate (UIPopupBase popupBase)
        {
            Pop(popupBase);
            if (popupBase.popupParam.msg == UIPopupMsg.Ok)
                if (action != null) action();
        };
    }

    public void StartRemoving(float sec)
    {
        StartCoroutine(RemovePopup(sec, this));
    }

    IEnumerator RemovePopup(float sec, UIPopupBase popup)
    {
        yield return new WaitForSeconds(sec);
        popup.PopupClose();
    }

    protected void OnExecuteMessage()
    {
        if (onPopupMessage != null)
            onPopupMessage(this);
    }

    public static UIPopupBase CurrentPopup()
    {
        int popupCount = listPopup.Count;
        if (popupCount > 0)
            return listPopup[popupCount - 1];
        return null;
    }
}
