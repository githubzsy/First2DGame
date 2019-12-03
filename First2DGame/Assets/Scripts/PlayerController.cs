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
    /// Bgm音乐
    /// </summary>
    [Header("Bgm音乐")]
    public AudioSource BgmAudio;
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
    /// 死亡时的音效
    /// </summary>
    [Header("死亡时的音效")]
    public AudioSource DeathAudio;

    /// <summary>
    /// 弹起来的速度
    /// </summary>
    [Header("弹起来时的速度")]
    public float BounceForce = 250;

    /// <summary>
    /// 是否蹲下了
    /// </summary>
    private bool _isCrouch = false;

    /// <summary>
    /// 玩家的头顶
    /// </summary>
    private Transform _celling;

    /// <summary>
    /// 跟着移动的相机
    /// </summary>
    [Header("跟着移动的相机")]
    public CinemachineVirtualCamera CinemachineVirtualCamera;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _celling = transform.Find("Celling");
    }

    void Update()
    {

        if (InputManager.IsJump() && _collider2D.IsTouchingLayers(Ground))
        { 
            _jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            _isCrouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            _isCrouch = false;
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
        else _animator.SetFloat("running",0);
    }

    void Jump()
    {
        //角色跳跃
        if (_jump)
        {
            JumpAudio.Play();
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpForce * Time.fixedDeltaTime);
            _animator.SetBool("jumping", true);
            _jump = false;
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
        else if (Physics2D.OverlapCircle(_celling.position, 0.5f, Ground)==false)
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
            CherryAudio.Play();
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
            BgmAudio.Stop();
            DeathAudio.Play();
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
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, BounceForce * Time.fixedDeltaTime);
                _animator.SetBool("jumping", true);
            }
            else
            {
                HurtAudio.Play();
                _isHurt = true;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x > 0 ? -3 : 3, _rigidbody2D.velocity.y);
            }
        }
    }
}
