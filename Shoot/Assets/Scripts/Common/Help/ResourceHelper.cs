using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceHelper : Singleton<ResourceHelper>
{
    class ResourceData : System.IDisposable
    {
        public Object resource = null;
        int count = 0;

        public ResourceData(Object res)
        {
            this.resource = res;
            this.count = 0;
        }

        public int Increase()
        {
            return ++count;
        }

        public int Decrease()
        {
            if (--count <= 0)
                this.Dispose();
            return count;
        }

        public void Dispose()
        {
            if (resource != null)
            {
                Resources.UnloadAsset(resource as GameObject);
                resource = null;
            }
        }
    }

    private Dictionary<string, Object> cacheResDic = new Dictionary<string, Object>();
    private Dictionary<string, Object> cacheBundleDic = new Dictionary<string, Object>();

    public static Object LoadObject(string resPath)
    {
        Object obj = null;

        ResourceHelper instance = Instance;
        if (instance != null)
        {
            if (instance.cacheResDic.ContainsKey(resPath))
                obj = instance.cacheResDic[resPath];
            else
            {
                obj = Resources.Load(resPath);
                if (obj != null)
                    instance.cacheResDic[resPath] = obj;
                else
                    Debug.LogWarning("can't load Object : " + resPath);
            }
        }
        return obj;
    }

    public static T LoadObject<T>(string resPath) where T : Object
    {
        T obj = default(T);

        ResourceHelper instance = Instance;
        if (instance != null)
        {
            if (instance.cacheResDic.ContainsKey(resPath))
                obj = (T)instance.cacheResDic[resPath];
            else
            {
                obj = Resources.Load(resPath, typeof(T)) as T;
                if (obj != null)
                    instance.cacheResDic[resPath] = obj;
                else
                    Debug.LogWarning("can't load Object : " + resPath);
            }
        }
        return obj;
    }

    public static GameObject LoadPrefab(string prefabPath)
    {
        return (GameObject)LoadObject(prefabPath);
    }

    public static void UnLoadPrefab(string prefabPath)
    {
        ResourceHelper instance = Instance;
        if (instance != null)
        {
            if (instance.cacheResDic.ContainsKey(prefabPath))
                instance.cacheResDic.Remove(prefabPath);
        }
    }

    public static Sprite LoadSprite(string path)
    {
        return LoadObject<Sprite>(path);
    }

    public static void UnLoad(string path)
    {
        ResourceHelper instance = Instance;
        if (instance != null)
        {
            if (instance.cacheResDic.ContainsKey(path))
                instance.cacheResDic.Remove(path);
        }
    }

    public static void ClearCache()
    {
        ResourceHelper instance = Instance;
        if (instance != null)
        {
            instance.cacheBundleDic.Clear();
            instance.cacheResDic.Clear();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }

    public static byte[] LoadGameData(string assetName)
    {
        string assetPath = "Assets/Resources/Data/";
#if UNITY_EDITOR
        Debug.Log("Load Assets Path = " + assetName);
        TextAsset asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + assetName + ".bytes");
        if (asset == null) return null;
        return asset.bytes;
#endif

        TextAsset ta = Resources.Load<TextAsset>("Data/" + assetName);
        if (ta == null) return null;
        return ta.bytes;
    }

    public static GameObject LoadFX(string assetName)
    {
        return null;
    }
}
