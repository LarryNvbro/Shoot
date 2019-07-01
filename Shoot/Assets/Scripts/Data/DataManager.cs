using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private BRDataContainer m_Container;

    public override void Init()
    {
        base.Init();
        m_Container = new BRDataContainer();
        m_Container.LoadAllData();
    }

    public BRStageInfo GetStageInfo(int id)
    {
        return m_Container.StageInfo.Find(stage => stage.ID == id);
    }

    public BRBrick GetBrickData(int id)
    {
        return m_Container.Brick.Find(item => item.ID == id);
    }

    public BRAttackItem GetAttackItem(int id)
    {
        return m_Container.AttackItem.Find(item => item.ID == id);
    }

    public List<List<BRBrick>> GetStageData(int stageId)
    {
        BRStageInfo info = GetStageInfo(stageId);
        int group = info.StageData;
        int dataID = group;
        List<List<BRBrick>> stage = new List<List<BRBrick>>();
        while (true)
        {
            BRStageData data = m_Container.StageData.Find(item => item.ID == dataID);
            if (data == null || data.Group != group) break;

            List<BRBrick> brickList = new List<BRBrick>();
            for (int i = 1; i <= info.SizeX; ++i)
            {
                int brickNo = (int)data.GetType().GetField(string.Format("Col{0}", i.ToString())).GetValue(data);
                if (brickNo != 0)
                {
                    int brickID = (int)info.GetType().GetField(string.Format("Brick{0}", brickNo.ToString())).GetValue(info);
                    brickList.Add(Instance.GetBrickData(brickID));
                }
                else
                {
                    brickList.Add(null);
                }
            }
            stage.Add(brickList);
            dataID++;
            if (dataID - group >= info.SizeY) break;
        }

        return stage;
    }

    public BRDropItem GetDropItem(int id)
    {
        return m_Container.DropItem.Find(item => item.ID == id);
    }

    public int GetStageCount()
    {
        return m_Container.StageInfo.Count;
    }
}
