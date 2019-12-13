using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 延迟类
/// </summary>
public static class  DelayToInvoke
{
    /// <summary>
    /// 延迟执行某个方法
    /// </summary>
    /// <param name="action">方法</param>
    /// <param name="delaySeconds">延迟时间</param>
    /// <returns></returns>
    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}
