using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    #region Contents
    MapManager _map;
    ObjectManager _obj;
    NetworkManager _network;
    MatchManager _match;

    public static MapManager Map { get { return Instance._map; } }
    public static ObjectManager Object { get { return Instance._obj; } }
    public static NetworkManager Network { get { return Instance._network; } }
    public static MatchManager Match { get {  return Instance._match; } }
	#endregion

	#region Core
	DataManager _data;
    PoolManager _pool;
    ResourceManager _resource;
    SceneManagerEx _scene;

    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    #endregion

    void Awake()
    {
        _map = new MapManager();
        _obj = new ObjectManager();
        _network = new NetworkManager();
        _match = gameObject.AddComponent<MatchManager>();
        _data = new DataManager();
        _pool = new PoolManager();
        _resource = new ResourceManager();
        _scene = new SceneManagerEx();
    }
    void Start()
    {
        Init();
	}

    void Update()
    {
        _network.Update();
    }

    static void Init()
    {
        if (s_instance == null)
        {
			GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            //DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._network.Init();
            s_instance._data.Init();
            s_instance._pool.Init();
        }		
	}

    public static void Clear()
    {
        Scene.Clear();
        Pool.Clear();
        Match.Clear();
    }
}
