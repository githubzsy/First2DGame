using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储玩家属性
/// </summary>
public class PlayerAttribute
{
    /// <summary>
    /// 当前移动速度
    /// </summary>
    [Tooltip("当前移动速度")]
    public float Speed;

    /// <summary>
    /// 跳跃的力量
    /// </summary>
    [Tooltip("跳跃力量")]
    public float JumpForce;

    /// <summary>
    /// 樱桃数量
    /// </summary>
    [Tooltip("樱桃数量")]
    public int CherryCount;

    /// <summary>
    /// 弹起来的力量
    /// </summary>
    [Tooltip("弹起来的力量")]
    public float BounceForce;

    /// <summary>
    /// 空中可跳跃的次数
    /// </summary>
    [Tooltip("空中可跳跃的次数")]
    public int ExtraJumpCount;

    /// <summary>
    /// 当前Hp
    /// </summary>
    [Tooltip("当前Hp")]
    public int Hp;

    /// <summary>
    /// 最大Hp
    /// </summary>
    [Tooltip("最大Hp")]
    public int MaxHp;

    /// <summary>
    /// 保存时的场景
    /// </summary>
    [Tooltip("保存时的场景")]
    public int SaveSceneIndex;

    /// <summary>
    /// 保存时的位置
    /// </summary>
    [Tooltip("保存时的位置")] 
    public Vector3 SavePosition;
}
