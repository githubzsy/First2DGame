using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 管理每一种预制物和其生成的GameObject
/// </summary>
public class PrefabPool
{
    private readonly GameObject _prefab;

    private readonly int _poolSize;

    private readonly Queue<GameObject> _pool;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <param name="poolSize">预制体池大小</param>
    public PrefabPool(GameObject prefab, int poolSize)
    {
        this._prefab = prefab;
        this._poolSize = poolSize;
        this._pool = new Queue<GameObject>(_poolSize);
    }

    /// <summary>
    /// 从池子中取出对象
    /// </summary>
    /// <returns></returns>
    public GameObject PrefabPoolSpawn(Transform parent, Vector3 position, Quaternion quaternion)
    {
        GameObject obj = null;
        //遍历对象池中是否有可以使用的对象
        //有,就激活拿出来使用
        obj = _pool.FirstOrDefault(a => a.activeSelf == false);

        if (obj == null)
        {
            //如果将要超出存储池了,则移除顶部的并销毁
            if (_pool.Count >= _poolSize)
            {
                var removeObj = _pool.Dequeue();
                //todo 不能将所有Destroy放在同一帧执行
                GameObject.Destroy(removeObj.gameObject);
            }

            obj = Object.Instantiate(_prefab);
            _pool.Enqueue(obj);
        }

        obj.transform.position = position;
        obj.transform.rotation = quaternion;
        obj.transform.SetParent(parent);
        obj.SetActive(true);
        //通过子类实例化接口对象,子类的脚本组件继承并实现了接口中的方法
        //control里面存的是该子类实现的方法,如果要生成一些特效,或者其他游戏行为,那么就可以继承IControl,通过它来进行调用
        IControl control = obj.GetComponent<IControl>();
        control?.Spawn();
        return obj;
    }

    /// <summary>
    /// 回收游戏对象
    /// </summary>
    /// <param name="obj">当前游戏对象</param>
    public void PrefabPoolUnSpawn(GameObject obj)
    {
        IControl control = obj.GetComponent<IControl>();
        control?.UnSpawn();
        obj.SetActive(false);
    }

    /// <summary>
    /// 回收所有的游戏对象
    /// </summary>
    public void PrefabPoolUnSpawnAll()
    {
        //回收用于处于激活状态的游戏对象
        foreach (GameObject item in _pool)
        {
            if (item.activeSelf)
            {
                PrefabPoolUnSpawn(item);
            }
        }
    }

    /// <summary>
    /// 检查某个游戏对象是否在对象池中
    /// </summary>
    /// <returns><c>true</c>, if pool contains was subed, <c>false</c> otherwise.</returns>
    /// <param name="obj">Object.</param>
    public bool PrefabPoolContains(GameObject obj)
    {
        return _pool.Contains(obj);
    }

    /// <summary>
    /// 回收并且销毁对象池中所有对象
    /// </summary>
    public void ClearAll()
    {
        foreach (GameObject item in _pool)
        {
            if (item.activeSelf)
            {
                //进行回收
                PrefabPoolUnSpawn(item);
            }
            //进行移除
            GameObject.Destroy(item);
        }
    }
}
