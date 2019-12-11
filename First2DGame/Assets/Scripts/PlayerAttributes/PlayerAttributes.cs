using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储玩家属性
/// </summary>
[CreateAssetMenu(fileName = "PlayerAttributes",menuName = "Player/PlayerAttributes")]
public class PlayerAttributes : ScriptableObject
{
    /// <summary>
    /// 当前移动速度
    /// </summary>
    [Tooltip("当前移动速度")]
    public float Speed = 300f;

    /// <summary>
    /// 跳跃的力量
    /// </summary>
    [Tooltip("跳跃力量")]
    public float JumpForce = 6f;

    /// <summary>
    /// 樱桃数量
    /// </summary>
    [Tooltip("樱桃数量")]
    public int CherryCount = 0;

    /// <summary>
    /// 弹起来的力量
    /// </summary>
    [Tooltip("弹起来的力量")]
    public float BounceForce = 5f;

    /// <summary>
    /// 空中可跳跃的次数
    /// </summary>
    [Tooltip("空中可跳跃的次数")]
    public int ExtraJumpCount = 0;

    /// <summary>
    /// 当前Hp
    /// </summary>
    [Tooltip("当前Hp")]
    public int Hp = 5;

    /// <summary>
    /// 最大Hp
    /// </summary>
    [Tooltip("最大Hp")]
    public int MaxHp = 5;
}
