using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可交互物体基类
/// </summary>
public abstract class InteractiveBase : MonoBehaviour
{
    [Tooltip("当前玩家")]
    public PlayerController PlayerController;

    /// <summary>
    /// 此时是否能够进行交互
    /// </summary>
    private bool _canInteractive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canInteractive = true;
            OnTriggerEnter2DAfter(collision);
        }
    }

    /// <summary>
    /// 玩家进入当前物体时
    /// </summary>
    /// <param name="playerCollision"></param>
    protected virtual void OnTriggerEnter2DAfter(Collider2D playerCollision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canInteractive = false;
            OnTriggerExit2DAfter(collision);
        }
    }

    /// <summary>
    /// 玩家离开当前物体时
    /// </summary>
    /// <param name="playerCollision"></param>
    protected virtual void OnTriggerExit2DAfter(Collider2D playerCollision)
    {

    }

    /// <summary>
    /// 玩家进行交互时的动作
    /// </summary>
    protected abstract void PlayerInteractive();

    private void Update()
    {
        if (_canInteractive && InputManager.IsInteractive())
        {
            PlayerInteractive();
        }
    }
}
