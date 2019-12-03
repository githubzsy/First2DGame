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
    public Joystick MoveJoystick;

    private static Joystick _moveJoyStick;

    /// <summary>
    /// 互动的虚拟按键
    /// </summary>
    public Button InteractiveButton;
    /// <summary>
    /// 是否点击了交互按键
    /// </summary>
    private static bool _interactiveClick = false;

    /// <summary>
    /// 跳跃的虚拟按键
    /// </summary>
    public Button JumpButton;

    /// <summary>
    /// 是否点击了跳跃
    /// </summary>
    private static bool _jumpClick = false;

    /// <summary>
    /// 所有虚拟按键的父类
    /// </summary>
    public GameObject Joysticks;

    private bool _isMobile = false;

    void Awake()
    {
#if UNITY_ANDROID
        _isMobile = true;
#endif
        if (_isMobile)
        {
            Joysticks.SetActive(true);
        }

        JumpButton.onClick.AddListener(JumpButtonClick);
        InteractiveButton.onClick.AddListener(InteractiveButtonClick);
        _moveJoyStick = MoveJoystick;
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
        var result = _jumpClick;
        if (_jumpClick)
        {
            _jumpClick = false;
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
         var result = _interactiveClick;
        if (_interactiveClick)
        {
            _interactiveClick = false;
        }
        else
        {
            result = Input.GetButtonDown("Interactive");
        }

        return result;
    }

    public static float GetAxis(MoveAxis moveAxis)
    {
        var value = moveAxis == MoveAxis.Horizontal ? _moveJoyStick.Horizontal : _moveJoyStick.Vertical;
        return value != 0f ? value : Input.GetAxis(moveAxis.ToString());
    }


}

/// <summary>
/// 枚举移动方向
/// </summary>
public enum MoveAxis
{
    Horizontal
}
