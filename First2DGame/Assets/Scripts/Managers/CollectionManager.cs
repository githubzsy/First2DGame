using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionManager : MonoBehaviour
{
    private CollectionInits _collectionInits;

    private static CollectionManager _instance;

    private static string _collectionJson;
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        //当前场景收集物的存储路径
        _collectionJson = SceneManager.GetActiveScene().name + "/Collections.json";
        LoadCollections();
    }

    /// <summary>
    /// 读取收集物品
    /// </summary>
    void LoadCollections()
    {
        _collectionInits = SaveManager.ReadFormFile<CollectionInits>(_collectionJson);
    }

    /// <summary>
    /// 保存收集物
    /// </summary>
    internal static void SaveCollections()
    {
        _instance._collectionInits.SaveToFile(_collectionJson);
    }

    void Start()
    {
       InitCollections();
    }

    void InitCollections()
    {
        var collectionGroups = from a in _collectionInits.Collections
            where a.PickedUp == false
            group a by a.PrefabFileName
            into g
            select g;
        
        foreach (var g in collectionGroups)
        {
            var prefab = Resources.Load<GameObject>(g.Key);
            foreach (var collection in g.ToList())
            {
                var temp = Instantiate(prefab, collection.Position, Quaternion.identity);
                temp.transform.parent = this.gameObject.transform;
                collection.InteractiveBase = temp.GetComponent<InteractiveBase>();
            }
        }
    }


    internal static void PickedUp(InteractiveBase interactiveBase)
    {
        foreach (var collectionInit in _instance._collectionInits.Collections.Where(a => a.InteractiveBase == interactiveBase))
        {
            collectionInit.PickedUp = true;
        }
    }

    internal static void ResetAll()
    {
        foreach (var collectionInit in _instance._collectionInits.Collections)
        {
            collectionInit.PickedUp = false;
        }
    }
}
