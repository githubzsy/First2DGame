using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 对象池类
/// </summary>
public class PoolManager
{
    //单例
    private static PoolManager _instance;

    private PoolManager()
    {

    }

    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance=new PoolManager();
            }

            return _instance;
        }
    }

    //存储各类型的对象池的集合
    readonly Dictionary<string, PrefabPool> _poolDic = new Dictionary<string, PrefabPool>();

    private int _poolSize = 50;

    /// <summary>
    /// 获取对象池中游戏对象
    /// </summary>
    internal GameObject Spawn(string prefabName,string prefabPath, Transform parent, Vector3 position, Quaternion quaternion)
    {
        GameObject obj;
        //若不包含这个预制体
        if (!_poolDic.ContainsKey(prefabName))
        {
            //若超出对象池
            //则移除第一个预制物
            if (_poolDic.Count >= _poolSize)
            {
                string removeKey = _poolDic.Keys.First();
                _poolDic[removeKey].ClearAll();
                _poolDic.Remove(removeKey);
            }
            //从资源中加载预制体
            obj = Resources.Load<GameObject>(prefabPath);
            _poolDic.Add(prefabName, new PrefabPool(obj,50));
        }
        PrefabPool prefabPool = _poolDic[prefabName];
        return prefabPool.PrefabPoolSpawn(parent, position, quaternion);
    }

    /// <summary>
    /// 回收游戏对象
    /// </summary>
    /// <param name="obj">Object.</param>
    public void UnSpawn(GameObject obj)
    {
        foreach (PrefabPool item in _poolDic.Values)
        {
            if (item.PrefabPoolContains(obj))
            {
                item.PrefabPoolUnSpawn(obj);
                break;
            }
        }
    }

    /// <summary>
    /// 清除游戏对象
    /// </summary>
    public void ClearPool()
    {
        _poolDic?.Clear();
    }
}
