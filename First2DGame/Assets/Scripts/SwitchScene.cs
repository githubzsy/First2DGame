using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 切换场景
/// </summary>
public class SwitchScene : MonoBehaviour
{
    private string _nextSceneName;

    /// <summary>
    /// 设置下一个场景的名称
    /// </summary>
    /// <param name="sceneName"></param>
    internal void SetNextSceneName(string sceneName)
    {
        _nextSceneName = sceneName;
    }

    /// <summary>
    /// 清空下一个场景的名称
    /// </summary>
    internal void ClearNextSceneName()
    {
        _nextSceneName = null;
    }

    void Update()
    {
        //若按下了交互按键，则进入到房子中
        if (string.IsNullOrWhiteSpace(_nextSceneName) == false && InputManager.IsInteractive())
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
