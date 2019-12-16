using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 关卡管理
/// </summary>
public static class LevelManager
{
    /// <summary>
    /// 加载下一个场景
    /// </summary>
    public static void LoadNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
