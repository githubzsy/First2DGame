using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 玩家管理类
/// </summary>
public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// 当前刚体
    /// </summary>
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;

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

    private bool _isHurt;

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
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    /// <summary>
    /// 是否踩在地面上
    /// </summary>
    private bool _isGround;

    /// <summary>
    /// 剩余的空中可跳跃的次数
    /// </summary>
    private int _extraJumpRemain;

    /// <summary>
    /// 受伤后的无敌时间
    /// </summary>
    private float _invisibleTime = 0.7f;

    /// <summary>
    /// 可以受伤的时间
    /// </summary>
    private float _nextHurtTime;

    /// <summary>
    /// 玩家是否死了
    /// </summary>
    private bool _isDead = false;


    private PlayerAttribute _playerAttribute;

    /// <summary>
    /// 玩家本地化数据存储位置
    /// </summary>
    internal static string PlayerAttributeJson = "PlayerAttribute.json";

    private static PlayerManager _instance;
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _celling = transform.Find("Celling");
        _groundCheck = transform.Find("GroundCheck");
        _nextHurtTime = Time.time;
        LoadPlayer();
    }

    void Start()
    {
        AttachToOthers();
        if (!AudioManager.IsPlayingBgm())
        {
            AudioManager.PlayBgm();
        }
        UIManager.RefreshHp(_playerAttribute.Hp);
        UIManager.RefreshCherryCount(_playerAttribute.CherryCount);
    }

    /// <summary>
    /// 读取玩家信息
    /// </summary>
    static void LoadPlayer()
    {
        _instance._playerAttribute = SaveManager.ReadFormFile<PlayerAttribute>(PlayerAttributeJson);
        if (SceneManager.GetActiveScene().buildIndex == _instance._playerAttribute.SaveSceneIndex)
        {
            _instance.transform.position = _instance._playerAttribute.SavePosition;
        }
    }

    /// <summary>
    /// 保存玩家信息
    /// </summary>
    internal static void SavePlayer()
    {
        _instance._playerAttribute.SaveSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _instance._playerAttribute.SavePosition = _instance.transform.position;
        _instance._playerAttribute.SaveToFile(PlayerAttributeJson);
    }

    /// <summary>
    /// 将玩家添加到其它脚本引用上
    /// </summary>
    void AttachToOthers()
    {
        _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (_cinemachineVirtualCamera != null)
        {
            _cinemachineVirtualCamera.Follow = transform;
        }

        DeadLine deadLine = FindObjectOfType<DeadLine>();
        if (deadLine != null)
        {
            deadLine.PlayerController = this;
        }

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
            _rigidbody2D.velocity = new Vector2(h * _playerAttribute.Speed * Time.fixedDeltaTime, _rigidbody2D.velocity.y);
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
            _extraJumpRemain = _playerAttribute.ExtraJumpCount;
        }

        //如果按下了跳跃键
        if (InputManager.IsJump())
        {
            //若在地面则可以跳跃
            if (_isGround)
            {
                AudioManager.JumpAudio();
                _rigidbody2D.velocity = Vector2.up * _playerAttribute.JumpForce;
                _animator.SetBool("jumping", true);
            }
            //否则空中跳跃次数大于0时也可以跳跃
            else if (_extraJumpRemain > 0)
            {
                AudioManager.JumpAudio();
                _rigidbody2D.velocity = Vector2.up * _playerAttribute.JumpForce;
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
    /// 重新加载当前场景
    /// </summary>
    private void Reset()
    {
        //重新加载当前场景
        var op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        op.completed += Reset_completed;
    }

    /// <summary>
    /// 场景加载完成后玩家生命值回满
    /// </summary>
    /// <param name="obj"></param>
    private void Reset_completed(AsyncOperation obj)
    {
        _playerAttribute.Hp = _playerAttribute.MaxHp;
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
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _playerAttribute.BounceForce);
                _animator.SetBool("jumping", true);
            }
            //如果超出了无敌时间
            else if (_nextHurtTime < Time.time)
            {
                AudioManager.HurtAudio();
                _isHurt = true;
                _playerAttribute.Hp--;
                UIManager.RefreshHp(_playerAttribute.Hp);
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x > 0 ? -3 : 3, _rigidbody2D.velocity.y);
                if (_playerAttribute.Hp <= 0)
                {
                    PlayerDie();
                }
                _nextHurtTime = Time.time + _invisibleTime;
            }
        }
    }

    /// <summary>
    /// 拾取樱桃
    /// </summary>
    /// <param name="cherry"></param>
    internal static void PickCherry(GameObject cherry)
    {
        Destroy(cherry);
        _instance._playerAttribute.CherryCount++;
        if (_instance._playerAttribute.CherryCount % 10 == 0)
        {
            _instance._playerAttribute.MaxHp++;
            _instance._playerAttribute.Hp++;
            UIManager.RefreshHp(_instance._playerAttribute.Hp);
            AudioManager.MaxHpIncreaseAudio();
        }
        else AudioManager.CherryAudio();

        UIManager.RefreshCherryCount(_instance._playerAttribute.CherryCount);
    }

    /// <summary>
    /// 获取到了技能
    /// </summary>
    /// <param name="skill"></param>
    internal static void PickSkill(GameObject skill)
    {
        AudioManager.SkillAudio();
        Destroy(skill);
    }


    /// <summary>
    /// 玩家死亡
    /// </summary>
    internal static void PlayerDie()
    {
        //保证玩家死亡效果不会重复触发
        if (_instance != null && _instance._isDead == false)
        {
            _instance._isDead = true;
            if (_instance._cinemachineVirtualCamera != null)
            {
                //相机不再移动
                _instance._cinemachineVirtualCamera.enabled = false;
            }

            _instance._collider2D.enabled = false;
            _instance._boxCollider2D.enabled = false;
            _instance._animator.SetBool("hurt", true);
            AudioManager.DeathAudio();
            AudioManager.StopBgm();
            _instance.Invoke("Reset", 2f);
            _instance = null;
        }
    }

    /// <summary>
    /// 增加一次额外跳跃的能力
    /// </summary>
    internal void ExtraJumpIncrease()
    {
        _instance._playerAttribute.ExtraJumpCount++;
    }

    internal int GetSaveSceneIndex()
    {
        return _playerAttribute.SaveSceneIndex;
    }
}
