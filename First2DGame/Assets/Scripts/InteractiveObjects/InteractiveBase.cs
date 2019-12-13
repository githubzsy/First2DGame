using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可交互物体基类
/// </summary>
[Serializable]
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
        //因为玩家身上有两个Collider，这里只用圆形的触发即可
        if (collision.CompareTag("Player") && collision is CircleCollider2D)
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
        if (collision.CompareTag("Player") && collision is CircleCollider2D)
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
