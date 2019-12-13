using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 收集物品初始化
/// </summary>
[Serializable]
public class CollectionInit
{
    public Vector3 Position;

    public bool PickedUp;

    /// <summary>
    /// 预制物所在文件
    /// </summary>
    public string PrefabFileName;

    internal InteractiveBase InteractiveBase;

}
