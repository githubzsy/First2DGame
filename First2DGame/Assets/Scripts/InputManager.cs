using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理玩家的输入
/// </summary>
public class InputManager:MonoBehaviour
{
    /// <summary>
    /// 移动的虚拟手柄
    /// </summary>
    private Joystick _moveJoystick;

    /// <summary>
    /// 互动的虚拟按键
    /// </summary>
    private Button _interactiveButton;
    /// <summary>
    /// 是否点击了交互按键
    /// </summary>
    private bool _interactiveClick = false;

    /// <summary>
    /// 跳跃的虚拟按键
    /// </summary>
    private Button _jumpButton;

    /// <summary>
    /// 是否点击了跳跃
    /// </summary>
    private bool _jumpClick = false;

    /// <summary>
    /// 所有虚拟按键的父类
    /// </summary>
    public GameObject Joysticks;

    private bool _isMobile = false;

    private static InputManager _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }


#if UNITY_ANDROID
        _isMobile = true;
#endif
        if (_isMobile)
        {
            Joysticks.SetActive(true);
        }

        _jumpButton = Joysticks.transform.Find("Jump").GetComponent<Button>();
        _interactiveButton = Joysticks.transform.Find("Interactive").GetComponent<Button>();
        _moveJoystick = Joysticks.transform.Find("MoveJoystick").GetComponent<Joystick>();

        _instance = this;
    }

    private void Start()
    {
        _jumpButton.onClick.AddListener(JumpButtonClick);
        _interactiveButton.onClick.AddListener(InteractiveButtonClick);
    }

    /// <summary>
    /// 一帧执行完后将按钮点击回归
    /// </summary>
    private void LateUpdate()
    {
        _jumpClick = false;
        _interactiveClick = false;
    }

    private void InteractiveButtonClick()
    {
        _interactiveClick = true;
    }

    private void JumpButtonClick()
    {
        _jumpClick = true;
    }

    /// <summary>
    /// 玩家是否按下了跳键
    /// </summary>
    /// <returns></returns>
    public static bool IsJump()
    {
        var result = _instance._jumpClick;
        if (_instance._jumpClick)
        {
            _instance._jumpClick = false;
        }
        else
        {
            result = Input.GetButtonDown("Jump");
        }

        return result;
    }

    /// <summary>
    /// 玩家是否按下了互动键
    /// </summary>
    /// <returns></returns>
    public static bool IsInteractive()
    {
        var result = _instance._interactiveClick;
        if (_instance._interactiveClick)
        {
            _instance._interactiveClick = false;
        }
        else
        {
            result = Input.GetButtonDown("Interactive");
        }

        return result;
    }

    public static float GetAxis(MoveAxis moveAxis)
    {
        var value = moveAxis == MoveAxis.Horizontal ? _instance._moveJoystick.Horizontal : _instance._moveJoystick.Vertical;
        if (value == 0)
        {
            value = Input.GetAxis(moveAxis.ToString());
        }
        return value;
    }

    /// <summary>
    /// 是否蹲下了
    /// </summary>
    /// <returns></returns>
    public static bool IsCrouch()
    {
        bool result;
        //1.控制摇杆向下超过0.3返回true
        result = _instance._moveJoystick.Vertical < -0.3;

        //2.按下Crouch键返回true
        if (result == false)
        {
            result = Input.GetButton("Crouch");
        }

        return result;
    }
}

/// <summary>
/// 枚举移动方向
/// </summary>
public enum MoveAxis
{
    Horizontal,
    Vertical
}
