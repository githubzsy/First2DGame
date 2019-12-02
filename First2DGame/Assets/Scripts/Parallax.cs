using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("视差移动速度,0表示背景固定(没有视差),1表示与摄像机同速(背景固定)")]
    public float MoveRate;

    [Header("是否要锁定Y轴")]
    public bool LockY;

    /// <summary>
    /// 当前背景的起始X坐标
    /// </summary>
    private float _startPointX;

    /// <summary>
    /// 起始Y坐标
    /// </summary>
    private float _startPointY;
    void Start()
    {
        _startPointX = transform.position.x;
        _startPointY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(_startPointX + Camera.main.transform.position.x * MoveRate, LockY ? transform.position.y : (_startPointY + Camera.main.transform.position.y * MoveRate));
    }
}
