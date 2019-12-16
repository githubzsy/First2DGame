using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControl
{
    /// <summary>
    /// 从对象池取出时
    /// </summary>
    void Spawn();

    /// <summary>
    /// 丢进对象池时
    /// </summary>
    void UnSpawn();
}
