using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 当前刚体
    /// </summary>
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;

    /// <summary>
    /// 初始速度
    /// </summary>
    [Header("初始速度")]
    public float Speed = 300f;

    /// <summary>
    /// 跳跃的力量
    /// </summary>
    [Header("跳跃的力量")] 
    public float JumpForce = 300f;

    /// <summary>
    /// 是否需要跳起
    /// </summary>
    private bool _jump = false;

    /// <summary>
    /// 地面
    /// </summary>
    [Header("当前地面图层")]
    public LayerMask Ground;

    /// <summary>
    /// 碰撞体
    /// </summary>
    private CircleCollider2D _collider2D;

    /// <summary>
    /// 樱桃数量
    /// </summary>
    public int CherryCount = 0;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        SwitchAnim();
    }

    void Movement()
    {
        //横向移动
        var h = Input.GetAxis("Horizontal");
        var direction = Input.GetAxisRaw("Horizontal");

        //角色移动
        if (h != 0)
        {
            _rigidbody2D.velocity = new Vector2(h * Speed * Time.deltaTime, _rigidbody2D.velocity.y);
            _animator.SetFloat("running",Mathf.Abs(direction));
        }

        //角色方向
        if (direction != 0)
        {
            transform.localScale=new Vector3(direction,transform.localScale.y);
        }

        //角色跳跃
        if (_jump)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpForce * Time.deltaTime);
            _animator.SetBool("jumping",true);
            _animator.SetBool("idle", false);
            _jump = false;
        }
    }

    /// <summary>
    /// 切换动画效果
    /// </summary>
    void SwitchAnim()
    {
        //如果正在跳跃
        if (_animator.GetBool("jumping"))
        {
            //若Y轴的力小于0
            if (_rigidbody2D.velocity.y < 0)
            {
                _animator.SetBool("jumping",false);
                _animator.SetBool("falling",true);
            }
        }
        //如果碰撞到了地面
        else if (_collider2D.IsTouchingLayers(Ground))
        {
            _animator.SetBool("falling",false);
            _animator.SetBool("idle",true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            CherryCount++;
        }
    }
}
