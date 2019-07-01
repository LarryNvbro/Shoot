#if UNITY_EDITOR
#define USE_CELLNAME_INDEX
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIReuseCell : MonoBehaviour
{
	[SerializeField]
	private int index;
	public int Index { get { return index; } set { index = value; } }
}



public class UIGridReuse : MonoBehaviour
{
	public const int AddLineCellCount = 3;
	public delegate GameObject MakeReuseCell(GameObject reuseCell, int index);

    [SerializeField]
	protected UIScrollView ScrollView;
	protected UIPanel ScrollPanel;
	protected Transform ScrollTrans;

	public UIWidget.Pivot pivot = UIWidget.Pivot.TopLeft;

	public int maxColumn = 1;
	public float cellWidth = 200f;
	public float cellHeight = 200f;


	protected Transform mTransform;
	protected Vector3 mStartPosition = Vector3.zero;
	protected Vector2 mStartOffset = Vector2.zero;
    protected Vector2 mLastOffset = Vector2.zero;

	[SerializeField]
	protected int maxDataCount = 0;
	public int MaxDataCount
	{
		get { return maxDataCount; }
		set { maxDataCount = Mathf.Max(0, value); }
	}

	List<UIReuseCell> mCellList = new List<UIReuseCell>();
	public List<UIReuseCell> reuseCellList { get { return mCellList; } }

	protected MakeReuseCell onMakeReuseCell;
	public UIProgressBar onProgressBar;


	/// <summary>
	/// 재사용 라인 개수
	/// </summary>
	protected int maxLineCount
	{
		get { return viewLineCount + AddLineCellCount;}
	}

	/// <summary>
	/// 화면에 보일 수 있는 라인 개수
	/// </summary>
	protected int viewLineCount
	{
		get
		{
			if (ScrollView.movement == UIScrollView.Movement.Vertical)
				return Mathf.CeilToInt(ScrollPanel.height / cellHeight);
			else
				return Mathf.CeilToInt(ScrollPanel.width / cellWidth);
		}
	}

	/// <summary>
	/// 화면에 보일 수 있는 가로 개수
	/// </summary>
	protected int maxCol
	{
		get
		{
			if (ScrollView.movement == UIScrollView.Movement.Vertical)
				return maxColumn;
			else
				return Mathf.CeilToInt(ScrollPanel.width / cellWidth);
		}
	}

	/// <summary>
	/// 화면에 보일 수 있는 세로 개수
	/// </summary>
	protected int maxRow
	{
		get
		{
			if (ScrollView.movement == UIScrollView.Movement.Vertical)
				return Mathf.CeilToInt(ScrollPanel.height / cellHeight);
			else
				return maxColumn;
		}
	}

	public float progress
	{
		get
		{
			return GetProgress();
		}

		set
		{
			SetProgress(value);
		}

	}

	/// <summary>
	/// 머리를 가리킨다.
	/// </summary>
	protected UIReuseCell CellHead { get { return mCellList.Count <= 0 ? null : mCellList[0]; } }

    /// <summary>
    /// 꼬리를 가리킨다.
    /// </summary>
    protected UIReuseCell CellTail { get { return mCellList.Count <= 0 ? null : mCellList[mCellList.Count - 1]; } }
	void Awake()
	{
		InitValidate();

		if (ScrollPanel != null)
		{
			ScrollPanel.ConstrainTargetToBounds(transform, true);
			UIScrollView sv = ScrollPanel.GetComponent<UIScrollView>();
			if (sv != null) sv.UpdateScrollbars(true);
		}

	}

	/*void Start()
	{

	}*/


	public void OnDestroy()
	{
		mCellList.Clear();
	}


	public void Update()
	{
		UpdateReusePosition();
	}


	protected void InitValidate()
	{
		this.enabled = true;

		mTransform = this.transform;
		if (ScrollView == null) ScrollView = NGUITools.FindInParents<UIScrollView>(this.gameObject);
		if (ScrollPanel == null) ScrollPanel = NGUITools.FindInParents<UIPanel>(this.gameObject);
		ScrollTrans = ScrollView.transform;
		maxColumn = maxColumn < 1 ? 1 : maxColumn;

		ScrollView.onDragStarted = OnDragStarted;
		ScrollView.onStoppedMoving = OnStoppedMoving;

		mStartPosition = ScrollTrans.localPosition;
		mStartOffset = ScrollPanel.finalClipRegion;
	}

	protected virtual void ClearCellList()
	{
		mCellList.Clear();
		for (int i = mTransform.childCount - 1; i >= 0; --i)
		{
			NGUITools.Destroy(mTransform.GetChild(i).gameObject);
		}
		mTransform.DetachChildren();
	}

	protected virtual void CreateReuseCell(int startIndex)
	{
		ClearCellList();

		int makeCount = maxLineCount * maxColumn;
		makeCount = Mathf.Min(makeCount, MaxDataCount);

		for (int i = 0; i < makeCount; ++i)
		{
			int index = i + startIndex;
			GameObject newCell = onMakeReuseCell(null, index);
			if (newCell == null)
				newCell = MakeCellEmpty(newCell, index);

			UIReuseCell reuseCell = newCell.GetComponent<UIReuseCell>();
			if (reuseCell == null)
				reuseCell = newCell.AddComponent<UIReuseCell>();

			reuseCell.Index = index;
			mCellList.Add(reuseCell);

			AddChildObject(reuseCell.gameObject);
		}

		UpdateCellPosition();
	}

	public void InitGrid(int listCount, MakeReuseCell makeReuseCell)
	{
		ClearGrid();

		InitValidate();

		MaxDataCount = listCount;
		onMakeReuseCell = makeReuseCell == null ? MakeCellEmpty : makeReuseCell;

		CreateReuseCell(0);

		ScrollView.ResetPosition();
		//ScrollView.InvalidateBounds();
		//ScrollView.RestrictWithinBounds(true);

		//UpdateScrollbars();

		UpdateProgressBar();

		mStartPosition = ScrollTrans.localPosition;
		mStartOffset = ScrollPanel.finalClipRegion;
    }

    public void AddGrid(int listCount)
    {
        MaxDataCount = listCount;
        UpdateProgressBar();
    }

	// 버그 테스트 필요
	private void SetReCountItem(int listCount)
	{
		MaxDataCount = listCount;

		int makeCount = maxLineCount * maxColumn;
		makeCount = Mathf.Min(makeCount, MaxDataCount);

		int add_count = makeCount - mCellList.Count;

		// add item
		if (add_count > 0)
		{
			int index = mCellList.Count;
			for(int i=0; i<add_count; ++i)
			{
				GameObject newCell = onMakeReuseCell(null, index);
				if (newCell == null)
				{
					newCell = MakeCellEmpty(newCell, index);
				}
				UIReuseCell reuseCell = newCell.GetComponent<UIReuseCell>();
				if (reuseCell == null)
					reuseCell = newCell.AddComponent<UIReuseCell>();

				mCellList.Add(reuseCell);
				mCellList[index].Index = index;
				mCellList[index].name = "item " + index;

				AddChildObject(mCellList[index].gameObject);

				index++;
			}
		}
		else if(add_count < 0)
		{
			for(int i=add_count; i < 0; ++i)
			{
				int index = mCellList.Count -1;
				NGUITools.Destroy(mCellList[index].gameObject);
				mCellList.RemoveAt(index);
			}
		}

		UpdateCellPosition();
		ReFresh();
	}



	public void ClearGrid()
	{
		MaxDataCount = 0;
		mCellList.Clear();
		mLastOffset = Vector2.zero;

		if(mTransform == null)
		{
			InitValidate();
		}

		ClearCellList();

		SetProgress(0.0f);
	}


	public void ReFresh()
	{
		for (int i = 0; i < mCellList.Count; i++)
		{
			onMakeReuseCell(mCellList[i].gameObject, mCellList[i].Index);
		}
	}

	protected GameObject MakeCellEmpty(GameObject reuse, int index)
	{
		if (reuse == null)
		{
			reuse = new GameObject();
		}
#if USE_CELLNAME_INDEX
		UILabel name = reuse.GetComponentInChildren<UILabel>();
		if (name != null) name.text = string.Format("item {0}", index);
#endif
		return reuse;
	}


	protected void OnDragStarted()
	{
		this.enabled = true;
	}

    protected void OnStoppedMoving()
	{
		this.enabled = false;
	}

	protected void AddChildObject(GameObject child)
	{
		Transform t = child.transform;
		t.parent = mTransform;
		t.localPosition = Vector3.zero;
		t.localRotation = Quaternion.identity;
		t.localScale = Vector3.one;
		child.layer = this.gameObject.layer;
		NGUITools.MarkParentAsChanged(child);
	}

	protected void UpdateScrollbars()
	{
		ScrollView.DisableSpring();
		ScrollView.UpdateScrollbars(true);
	}


	protected void UpdateProgressBar()
	{
		if (onProgressBar == null) return;
		onProgressBar.value = progress;

		UIScrollBar sb = onProgressBar as UIScrollBar;
		if (sb != null) 
		{
			if (ScrollView.movement == UIScrollView.Movement.Vertical)
			{
                float maxDataSize = GetMaxScrollHeight();
                sb.barSize = ScrollPanel.height / maxDataSize;
			}
			else
			{
                float maxDataSize = GetMaxScrollHeight();
                sb.barSize = ScrollPanel.width / maxDataSize;
			}
            // 알파값 지정
            onProgressBar.alpha = sb.barSize < 1 ? 1 : 0;
		}
	}

    protected virtual float GetMaxScrollHeight()
    {
        return Mathf.CeilToInt(MaxDataCount / (float)maxColumn) * cellHeight;
    }

    public void OnProgressChange()
    {
        if (onProgressBar == null) return;
        SpringPanel s_panel = ScrollView.GetComponent<SpringPanel>();
        if (s_panel != null && s_panel.enabled)
            s_panel.enabled = false;
        progress = onProgressBar.value;
    }

	protected Vector2 GetCurrentPosition()
	{
		Vector2 currentPos = ScrollPanel.finalClipRegion;
		currentPos = currentPos - mStartOffset;
		return currentPos;
	}


	public float GetProgress()
	{
		float scroll = 0.0f;
		Vector2 currentPos = GetCurrentPosition();

		if (ScrollView.movement == UIScrollView.Movement.Vertical)
		{
			float maxDataSize = GetMaxScrollHeight();
            float listSize = maxDataSize - ScrollPanel.height;
			scroll = -currentPos.y / listSize;
		}
		else
		{
			float maxDataSize = Mathf.CeilToInt(MaxDataCount / (float)maxColumn) * cellWidth;
			float listSize = maxDataSize - ScrollPanel.width;
			scroll = currentPos.x / listSize;
		}

		scroll = float.IsNaN(scroll) ? 0.0f : Mathf.Clamp(scroll, 0.0f, 1.0f);
		return scroll;
	}

	
	public void SetProgress(float scroll)
	{
		scroll = Mathf.Clamp(scroll, 0.0f, 1.0f);

		Vector2 offset = mStartOffset;
		Vector3 trans_positon = mStartPosition;

		if (ScrollView.movement == UIScrollView.Movement.Vertical)
		{
			float maxDataSize = GetMaxScrollHeight();
            float listSize = maxDataSize - ScrollPanel.height;
			if (listSize >= 0.0f)
			{
				float viewPosition = listSize * scroll;
				offset.y -= viewPosition;
				trans_positon.y += viewPosition;
			}
		}
		else
		{
			float maxDataSize = Mathf.CeilToInt(MaxDataCount / (float)maxColumn) * cellWidth;
			float listSize = maxDataSize - ScrollPanel.width;
			if (listSize >= 0.0f)
			{
				float viewPosition = listSize * scroll;
				offset.x += viewPosition;
				trans_positon.x -= viewPosition;
			}
		}

		ScrollPanel.clipOffset = offset;
		ScrollTrans.localPosition = trans_positon;

		UpdateReusePosition();
	}


	/// <summary>
	/// 아이템들의 재사용을 위하여 위치를 조절한다.
	/// </summary>
	public virtual void UpdateReusePosition()
	{
		if (mCellList.Count == 0)
		{
			this.enabled = false;
			return;
		}

		Vector2 currentPos = GetCurrentPosition();

		if (ScrollView.movement == UIScrollView.Movement.Vertical)
		{
			if (currentPos.y == mLastOffset.y)
			{
				return;
			}

			bool isMoveDown = currentPos.y > mLastOffset.y;
			int col = maxCol;

			if (isMoveDown)
			{
				//가로줄의 맨 처음 
				//int headIndex = (int)((-currentPos.y - (cellHeight * 0.5f) - (cellHeight)) / cellHeight) * col;
				int headIndex = (int)((-currentPos.y - (cellHeight * 1.5f)) / cellHeight) * col;
				headIndex = Mathf.Clamp(headIndex, 0, MaxDataCount - col);
				if (CellHead.Index > headIndex && headIndex <= MaxDataCount - mCellList.Count)
				{
					//Debug.Log(string.Format("Vertical headIndex = {0}, movePos = {1}", headIndex, currentPos));
					// 꼬리 -> 처음.
					TailToHead(headIndex);
					UpdateCellPosition();
					UpdateScrollbars();
				}
			}
			else
			{
				//가로줄의 맨 오른쪽
				int tailIndex = (Mathf.CeilToInt((-currentPos.y + ScrollPanel.height + (cellHeight)) / cellHeight) * col) - 1;
				tailIndex = Mathf.Clamp(tailIndex, 0, MaxDataCount - 1);
				if (CellTail.Index < tailIndex && tailIndex >= mCellList.Count)
				{
					//Debug.Log(string.Format("Vertical tailIndex = {0}, movePos = {1}", tailIndex, currentPos));
					// 처음 -> 꼬리.
					HeadToTail(tailIndex);
					UpdateCellPosition();
					UpdateScrollbars();
				}
			}
		}

		else
		{
			if (currentPos.x == mLastOffset.x)
			{
				return;
			}

			bool isMoveLeft = currentPos.x < mLastOffset.x;
			int row = maxRow;

			if (isMoveLeft)
			{
				//int headIndex = (int)((currentPos.x - (cellWidth * 0.5f) - (cellWidth)) / cellWidth) * row;
				int headIndex = (int)((currentPos.x - (cellWidth * 1.5f)) / cellWidth) * row;
				headIndex = Mathf.Clamp(headIndex, 0, MaxDataCount - row);
				if (CellHead.Index > headIndex && headIndex <= MaxDataCount - mCellList.Count)
				{
					//Debug.Log(string.Format("Horizontal headIndex = {0}, movePos = {1}", headIndex, currentPos));
					// 꼬리 -> 처음.
					TailToHead(headIndex);
					UpdateCellPosition();
					UpdateScrollbars();
				}
			}
			else
			{
				//세로줄의 맨 오른쪽
				int tailIndex = (Mathf.CeilToInt((currentPos.x + ScrollPanel.width + (cellWidth)) / cellWidth) * row) - 1;
				tailIndex = Mathf.Clamp(tailIndex, 0, MaxDataCount - 1);
				if (CellTail.Index < tailIndex && tailIndex >= mCellList.Count)
				{
					//Debug.Log(string.Format("Horizontal tailIndex = {0}, movePos = {1}", tailIndex, currentPos));
					// 처음 -> 꼬리.
					HeadToTail(tailIndex);
					UpdateCellPosition();
					UpdateScrollbars();
				}
			}

		}

		mLastOffset = currentPos;

		UpdateProgressBar();
	}

	/// <summary>
	/// 아이템들의 위치를 정한다.
	/// </summary>
	protected virtual void UpdateCellPosition()
	{
		float fx = 0.0f;
		float fy = 0.0f;

		Vector2 offsetPivot = NGUIMath.GetPivotOffset(pivot);

		if (ScrollView.movement == UIScrollView.Movement.Vertical)
		{
			int col = maxCol;
			fx = Mathf.Lerp(0.0f, (maxColumn - 1) * cellWidth, offsetPivot.x);

			Transform transCell;
			for (int i = 0; i < mCellList.Count; i++)
			{
				transCell = mCellList[i].transform;

				//index를 기준으로 위치를 다시 잡는다. ( % 연산)
				int div = mCellList[i].Index / col;
				int div_mod = mCellList[i].Index - (col * div);

				Vector3 position = Vector3.zero;
				position.x = -fx + (div_mod * cellWidth);
				position.y = -fy - (div * cellHeight);

				transCell.localPosition = position;
#if USE_CELLNAME_INDEX
				transCell.name = string.Format("item {0}", mCellList[i].Index);
#endif
			}
		}
		else
		{
			int row = maxRow;
			fy = Mathf.Lerp(-(maxColumn - 1) * cellHeight, 0f, offsetPivot.y);

			Transform transCell;
			for (int i = 0; i < mCellList.Count; i++)
			{
				transCell = mCellList[i].transform;

				//index를 기준으로 위치를 다시 잡는다. ( % 연산)
				int div = mCellList[i].Index / row;
				int div_mod = mCellList[i].Index - (row * div);

				Vector3 position = Vector3.zero;
				position.x = -fx + (div * cellWidth);
				position.y = -fy - (div_mod * cellHeight);

				transCell.localPosition = position;
#if USE_CELLNAME_INDEX
				transCell.name = string.Format("item {0}", mCellList[i].Index);
#endif
			}
		}
	}


	/// <summary>
	/// 머리 부분을 꼬리 부분으로 옮긴다. 
	/// </summary>
	protected void HeadToTail(int toIndex)
	{
		UIReuseCell itemHead;
		UIReuseCell itemTail;

		//int cnt = ScrollView.movement == UIScrollView.Movement.Vertical ? maxCol : maxRow;

		while (true)
		{
			itemHead = CellHead;
			itemTail = CellTail;

			if (itemHead == null) return;
			if (itemTail == null) return;
			if (itemTail.Index >= toIndex) return;

			itemHead.Index = itemTail.Index + 1;

			mCellList.RemoveAt(0);
			mCellList.Insert(mCellList.Count, itemHead);

			onMakeReuseCell(itemHead.gameObject, itemHead.Index);
#if USE_CELLNAME_INDEX
			itemHead.name = string.Format("item {0}", itemHead.Index);
#endif
		}
	}

	/// <summary>
	/// 꼬리부분을 머리부분으로 옮긴다. 
	/// </summary>
	protected void TailToHead(int toIndex)
	{
		UIReuseCell itemHead;
		UIReuseCell itemTail;

		//int cnt = ScrollView.movement == UIScrollView.Movement.Vertical ? maxCol : maxRow;

		while (true)
		{
			itemHead = CellHead;
			itemTail = CellTail;

			if (itemTail == null) return;
			if (itemHead == null) return;
			if (itemHead.Index <= toIndex) return;

			itemTail.Index = itemHead.Index - 1;

			mCellList.RemoveAt(mCellList.Count - 1);
			mCellList.Insert(0, itemTail);

			onMakeReuseCell(itemTail.gameObject, itemTail.Index);
#if USE_CELLNAME_INDEX
			itemTail.name = string.Format("item {0}", itemTail.Index);
#endif
		}

	}

}
