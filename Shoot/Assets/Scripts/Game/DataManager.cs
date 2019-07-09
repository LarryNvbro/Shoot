using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private SHDataContainer m_Container = new SHDataContainer();

    public override void Init()
    {
        m_Container.LoadAllData();
    }

    public SHEnemy GetEnemy(int id)
    {
        return m_Container.Enemy.Find(item => item.ID == id);
    }

    public SHStageInfo GetStageInfo(int id)
    {
        return m_Container.StageInfo.Find(item => item.ID == id);
    }

    public SHStageData GetStageData(int id)
    {
        return m_Container.StageData.Find(item => item.ID == id);
    }
}
