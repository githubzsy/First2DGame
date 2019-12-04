using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float JumpForce = 6f;

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
    /// 弹起来的速度
    /// </summary>
    [Header("弹起来时的速度")]
    public float BounceForce = 5f;

    /// <summary>
    /// 是否蹲下了
    /// </summary>
    private bool _isCrouch = false;

    /// <summary>
    /// 玩家的头顶检测点
    /// </summary>
    private Transform _celling;

    /// <summary>
    /// 头顶检测半径
    /// </summary>
    private float _cellingCheckRadius = 0.5f;

    /// <summary>
    /// 玩家脚步检测点
    /// </summary>
    private Transform _groundCheck;

    /// <summary>
    /// 脚本检测半径范围
    /// </summary>
    private float _groundCheckRadius = 0.3f;

    /// <summary>
    /// 跟着移动的相机
    /// </summary>
    [Header("跟着移动的相机")]
    public CinemachineVirtualCamera CinemachineVirtualCamera;

    /// <summary>
    /// 是否踩在地面上
    /// </summary>
    private bool _isGround;

    /// <summary>
    /// 空中可跳跃的次数
    /// </summary>
    private int _extraJumpRemain;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _celling = transform.Find("Celling");
        _groundCheck = transform.Find("GroundCheck");
    }

    void Update()
    {
        _isGround = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, Ground);

        _isCrouch = InputManager.IsCrouch();

        //如果没有受伤则移动
        if (_isHurt == false)
        {
            Movement();
        }

        SwitchAnim();
    }

    void Movement()
    {
        Run();

        Crouch();

        Jump();
    }

    /// <summary>
    /// 跑步
    /// </summary>
    private void Run()
    {
        //横向移动
        var h = InputManager.GetAxis(MoveAxis.Horizontal);

        //角色移动
        if (h != 0)
        {
            _rigidbody2D.velocity = new Vector2(h * Speed * Time.fixedDeltaTime, _rigidbody2D.velocity.y);
            var hDirection = 0;
            if (h > 0) hDirection = 1;
            if (h < 0) hDirection = -1;
            _animator.SetFloat("running", 1);
            transform.localScale = new Vector3(hDirection, transform.localScale.y, transform.localScale.z);
        }
        else _animator.SetFloat("running", 0);
    }

    void Jump()
    {
        //若踩在地面上则恢复多段跳次数
        if (_isGround)
        {
            _extraJumpRemain = 1;
        }

        //如果按下了跳跃键
        if (InputManager.IsJump())
        {
            //若在地面则可以跳跃
            if (_isGround)
            {
                SoundManager.Instance.JumpAudio();
                _rigidbody2D.velocity = Vector2.up * JumpForce;
                _animator.SetBool("jumping", true);
            }
            //否则空中跳跃次数大于0时也可以跳跃
            else if (_extraJumpRemain > 0)
            {
                SoundManager.Instance.JumpAudio();
                _rigidbody2D.velocity = Vector2.up * JumpForce;
                _animator.SetBool("jumping", true);
                _extraJumpRemain--;
            }
        }
    }

    /// <summary>
    /// 蹲下的动作
    /// </summary>
    void Crouch()
    {
        if (_isCrouch)
        {
            _animator.SetBool("crouching", true);
            _boxCollider2D.enabled = false;
        }
        //若不是蹲下，且头顶的0.5半径的圆里面没有Ground，则站起来
        else if (Physics2D.OverlapCircle(_celling.position, _cellingCheckRadius, Ground) == false)
        {
            _animator.SetBool("crouching", false);
            _boxCollider2D.enabled = true;
        }
    }

    /// <summary>
    /// 切换动画效果
    /// </summary>
    void SwitchAnim()
    {
        //如果没有上升速度且在半空中
        if (_rigidbody2D.velocity.y < 0.1f && !_collider2D.IsTouchingLayers(Ground))
        {
            _animator.SetBool("falling", true);
        }
        //如果正在跳跃
        if (_animator.GetBool("jumping"))
        {
            //若Y轴的力小于0
            if (_rigidbody2D.velocity.y < 0)
            {
                _animator.SetBool("jumping", false);
                _animator.SetBool("falling", true);
            }
        }
        else if (_isHurt)
        {
            //受伤效果
            _animator.SetBool("hurt", true);
            //取消跑步的效果
            _animator.SetFloat("running", 0);
            //如果受伤的弹力小于0.1则认为受伤效果结束
            if (Mathf.Abs(_rigidbody2D.velocity.x) < 0.1f)
            {
                //取消受伤
                _animator.SetBool("hurt", false);
                _isHurt = false;
            }
        }
        //如果碰撞到了地面
        else if (_collider2D.IsTouchingLayers(Ground))
        {
            _animator.SetBool("falling", false);
        }
    }

    /// <summary>
    /// 2D触发器进入时
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            SoundManager.Instance.CherryAudio();
            Destroy(collision.gameObject);
            CherryCount++;
            CherryNum.text = CherryCount.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            if (CinemachineVirtualCamera != null)
            {
                //相机不再移动
                CinemachineVirtualCamera.enabled = false;
            }
            SoundManager.Instance.DeathAudio();
            SoundManager.Instance.StopBgm();
            Invoke("Reset", 2f);
        }
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    private void Reset()
    {
        //重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 碰撞到敌人
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果是下落时碰撞到敌人，则消灭敌人

        if (collision.gameObject.tag == "Enemy")
        {
            if (_animator.GetBool("falling"))
            {
                //青蛙被攻击了
                collision.gameObject.GetComponent<Enemy>().Attacked();
                //而且小跳一下
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, BounceForce);
                _animator.SetBool("jumping", true);
            }
            else
            {
                SoundManager.Instance.HurtAudio();
                _isHurt = true;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x > 0 ? -3 : 3, _rigidbody2D.velocity.y);
            }
        }
    }
}
