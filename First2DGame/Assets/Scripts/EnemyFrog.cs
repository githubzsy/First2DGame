using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyFrog : Enemy
{
    /// <summary>
    /// 移动左边界
    /// </summary>
    private float _leftX;

    /// <summary>
    /// 移动右边界
    /// </summary>
    private float _rightX;

    /// <summary>
    /// 是否面向左
    /// </summary>
    private bool _faceLeft = true;

    /// <summary>
    /// 移动的速度
    /// </summary>
    public float Speed = 3;

    public float JumpForce = 3;

    /// <summary>
    /// 初始的scaleX
    /// </summary>
    private float _initLocalScaleX;

    /// <summary>
    /// 地面
    /// </summary>
    [Header("当前地面图层")]
    public LayerMask Ground;

    /// <summary>
    /// 跳起的音效
    /// </summary>
    [Header("跳起的音效")]
    public AudioSource JumpAudio;
    protected override void Start()
    {
        base.Start();

        _initLocalScaleX = transform.localScale.x;
        var left = transform.Find("left");
        _leftX = left.transform.position.x;
        Destroy(left.gameObject);
        var right = transform.Find("right");
        _rightX = right.transform.position.x;
        Destroy(right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        //如果在地上则添加速度
        //速度方向与朝向相同
        if (Collider2D.IsTouchingLayers(Ground))
        {
            JumpAudio.Play();
            Animator.SetBool("jumping",true);
            Rigidbody2D.velocity = new Vector2(_faceLeft ? -Speed : Speed, JumpForce);
        }


        //若已到达边界，则转向
        if (transform.position.x < _leftX)
        {
            _faceLeft = false;
        }

        if (transform.position.x > _rightX)
        {
            _faceLeft = true;
        }

        //控制scale转向
        transform.localScale = new Vector3(_faceLeft ? _initLocalScaleX : -_initLocalScaleX, transform.localScale.y);
    }

    /// <summary>
    /// 切换动画
    /// </summary>
    void SwitchAnim()
    {
        //若处于下落状态
        if (Animator.GetBool("jumping"))
        {
            //且向上的力小于0.1
            if (Rigidbody2D.velocity.y < 0.1)
            {
                //jumping
                Animator.SetBool("jumping", false);
                //添加falling状态
                Animator.SetBool("falling", true);
            }
        }

        //若下落时，且碰到了地面
        if (Animator.GetBool("falling") && Collider2D.IsTouchingLayers(Ground))
        {
            //则取消falling状态
            Animator.SetBool("falling",false);
        }
    }
}
