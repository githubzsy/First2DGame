using System;
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
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1).completed += LoadScene_Completed;
    }

    public static void LoadScene(string sceneName)
    {
        var res = SceneManager.LoadSceneAsync(sceneName);
        res.completed += LoadScene_Completed;
    }       

    public static void LoadSceneAsync(string sceneName,Action<AsyncOperation> callBack)
    {
        var res = SceneManager.LoadSceneAsync(sceneName);
        res.completed += LoadScene_Completed;
        res.completed += callBack;
    }

    private static void LoadScene_Completed(AsyncOperation obj)
    {
        PoolManager.Instance.ClearPool();
    }
}
