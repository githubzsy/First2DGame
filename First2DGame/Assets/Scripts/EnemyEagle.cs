using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 老鹰的脚本
/// </summary>
public class EnemyEagle : Enemy
{
    /// <summary>
    /// 初始速度
    /// </summary>
    [Header("初始速度")]
    public float Speed = 2f;

    /// <summary>
    /// 向上飞
    /// </summary>
    private bool _flyUp = true;

    /// <summary>
    /// 向上飞的边界
    /// </summary>
    private float _topY;

    /// <summary>
    /// 向下飞的边界
    /// </summary>
    private float _bottomY;

    protected override void Awake()
    {
        base.Awake();

        var top = transform.Find("top");
        _topY = top.transform.position.y;
        Destroy(top.gameObject);
        var bottom = transform.Find("bottom");
        _bottomY = bottom.transform.position.y;
        Destroy(bottom.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {

        //若已到达边界，则转向
        if (transform.position.y < _bottomY)
        {
            _flyUp = true;
        }

        if (transform.position.y > _topY)
        {
            _flyUp = false;
        }

        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _flyUp ? Speed : -Speed);
    }
}
