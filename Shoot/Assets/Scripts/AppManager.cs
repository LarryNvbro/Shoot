using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class AppManager : Singleton<AppManager>
{
    private Vector2 m_TouchPos;

    public const string CryptDataKey = "N^br0C&yp#D@t@2018052987";
    private int m_SelectLevel = -1;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_TouchPos = Util.GetMousePosition();
        }
    }

    public override void Init()
    {
        base.Init();
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public Vector2 GetLastTouchPos()
    {
        return m_TouchPos;
    }

    /// <summary>
    /// 세이브 공용 데이터 저장
    /// </summary>
    public static void SetPublicPrefsCrypt(string keyName, string data)
    {
        if (data == null) return;

        string hashName = NVCrypt.GetMD5HashString(keyName);
        string filePath = string.Format("{0}/{1}", Application.persistentDataPath, hashName);

        byte[] bytes = UTF8Encoding.UTF8.GetBytes(data);
        byte[] encryptData = NVCrypt.AesEncrypt(bytes, CryptDataKey);

        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            fs.Write(encryptData, 0, encryptData.Length);
            fs.Flush();
            fs.Close();
        }
    }

    /// <summary>
    /// 세이브 공용 데이터 읽기
    /// </summary>
    public static string GetPublicPrefsCrypt(string keyName)
    {
        string hashName = NVCrypt.GetMD5HashString(keyName);
        string filePath = string.Format("{0}/{1}", Application.persistentDataPath, hashName);

        string ret = null;

        if (File.Exists(filePath))
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fs.CanRead)
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    ret = UTF8Encoding.UTF8.GetString(NVCrypt.AesDecrypt(bytes, CryptDataKey));
                }
                fs.Close();
            }
        }
        return ret;
    }

    public void SelectLevel(int level)
    {
        m_SelectLevel = level;
    }

    public int GetSelectLevel()
    {
        return m_SelectLevel;
    }
}
