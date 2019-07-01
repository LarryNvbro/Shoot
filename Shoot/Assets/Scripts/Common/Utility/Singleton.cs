using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T _instance;
	private static object _lock = new object();
	protected static bool applicationIsQuitting = false;
	public static bool IsApplicationQuitting
	{
		get
		{
			return applicationIsQuitting;
		}
	}

	void Awake()
	{
		lock (_lock)
		{
			if (_instance == null)
			{
				_instance = this as T;
				DontDestroyOnLoad(this.gameObject);
			}
			else if (_instance != this.GetComponent<T>())
			{
				Debug.LogError(this.name + " Destroy !!!");
				Destroy(this.gameObject);
			}
		}
	}

	public virtual void Init() { }

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}


			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = (T)FindObjectOfType(typeof(T));
					if (FindObjectsOfType(typeof(T)).Length > 1)
					{
						Debug.LogError("[Singleton] Something went really wrong " +
							" - there should never be more than 1 singleton!" +
							" Reopenning the scene might fix it.");
						return _instance;
					}
					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						//singleton.name = "[ " + typeof(T).ToString() + " ]";
						singleton.name = typeof(T).ToString();
						_instance.Init();
						DontDestroyOnLoad(singleton);

						Debug.Log("[Singleton] An instance of " + typeof(T) +
							" is needed in the scene, so '" + singleton +
							"' was created with DontDestroyOnLoad.");
					}
					else
					{
						Debug.Log("[Singleton] Using instance already created: " +
							_instance.gameObject.name);
					}
				}
				return _instance;
			}
		}
	}



	public virtual void OnApplicationQuit()
	{
		Debug.Log("OnApplicationQuit " + this.name);

		applicationIsQuitting = true;
	}
}
