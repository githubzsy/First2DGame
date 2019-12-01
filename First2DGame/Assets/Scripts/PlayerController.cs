using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    /// 身体碰撞体
    /// </summary>
    private CircleCollider2D _collider2D;

    /// <summary>
    /// 头部碰撞体
    /// </summary>
    private BoxCollider2D _boxCollider2D;

    /// <summary>
    /// 樱桃数量
    /// </summary>
    public int CherryCount = 0;

    [Header("樱桃数量文本")]
    public Text CherryNum;

    private bool _isHurt;

    /// <summary>
    /// 跳起时的声音
    /// </summary>
    [Header("跳起时的声音")]
    public AudioSource JumpAudio;

    /// <summary>
    /// 受伤时的音效
    /// </summary>
    [Header("受伤时的音效")] 
    public AudioSource HurtAudio;

    /// <summary>
    /// 捡起草莓时的音效
    /// </summary>
    [Header("捡起草莓时的音效")]
    public AudioSource CherryAudio;

    /// <summary>
    /// 弹起来的速度
    /// </summary>
    [Header("弹起来时的速度")]
    public float BounceForce = 250;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (_collider2D.IsTouchingLayers(Ground))
            {

                _jump = true;
            }
            else
            {
                //EditorApplication.isPaused = true;
            }
        }

        //站立状态且按下下键
        if (Input.GetAxisRaw("Vertical") < 0 && _collider2D.IsTouchingLayers(Ground))
        {
            _animator.SetBool("crouching", true);
            _boxCollider2D.enabled = false;
        }
        else
        {
            _animator.SetBool("crouching", false);
            _boxCollider2D.enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //如果没有受伤则移动
        if (_isHurt == false)
        {
            Movement();
        }

        SwitchAnim();
    }

    void Movement()
    {
        //横向移动
        var h = Input.GetAxis("Horizontal");
        var hDirection = Input.GetAxisRaw("Horizontal");

        //角色移动
        if (h != 0)
        {
            _rigidbody2D.velocity = new Vector2(h * Speed * Time.deltaTime, _rigidbody2D.velocity.y);
            _animator.SetFloat("running",Mathf.Abs(hDirection));
        }

        //角色方向
        if (hDirection != 0)
        {
            transform.localScale=new Vector3(hDirection,transform.localScale.y);
        }

       

        //角色跳跃
        if (_jump)
        {
            JumpAudio.Play();
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
        _animator.SetBool("idle",false);
        //如果没有上升速度且在半空中
        if (_rigidbody2D.velocity.y < 0.1f && !_collider2D.IsTouchingLayers(Ground))
        {
            _animator.SetBool("falling",true);
        }
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
        else if (_isHurt)
        {
            //受伤效果
            _animator.SetBool("hurt", true);
            //取消跑步的效果
            _animator.SetFloat("running",0);
            //如果受伤的弹力小于0.1则认为受伤效果结束
            if (Mathf.Abs(_rigidbody2D.velocity.x) < 0.1f)
            {
                //取消受伤
                _animator.SetBool("hurt", false);
                _animator.SetBool("idle",true);
                _isHurt = false;
            }
        }
        //如果碰撞到了地面
        else if (_collider2D.IsTouchingLayers(Ground))
        {
            _animator.SetBool("falling",false);
            _animator.SetBool("idle",true);
        }
    }

    /// <summary>
    /// 收集物体
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            CherryAudio.Play();
            Destroy(collision.gameObject);
            CherryCount++;
            CherryNum.text = CherryCount.ToString();
        }
    }

    /// <summary>
    /// 碰撞到敌人
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果是下落时碰撞到敌人，则消灭敌人

        if (collision.gameObject.tag == "Enemy" )
        {
            if (_animator.GetBool("falling"))
            {
                //青蛙被攻击了
                collision.gameObject.GetComponent<Enemy>().Attacked();
                //而且小跳一下
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, BounceForce * Time.fixedDeltaTime);
                _animator.SetBool("jumping", true);
                _animator.SetBool("idle", false);
            }
            else
            {
                HurtAudio.Play();
                _isHurt = true;
                _rigidbody2D.velocity=new Vector2(_rigidbody2D.velocity.x>0?-3:3,_rigidbody2D.velocity.y);
            }
        }
    }
}
