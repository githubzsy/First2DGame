using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 门的主函数
/// </summary>
public class Door : InteractiveBase
{
    /// <summary>
    /// 要切换的场景名称
    /// </summary>
    [Header("要切换的场景名称")]
    public string NextSceneName;

    /// <summary>
    /// 要提示的文字
    /// </summary>
    [Header("要提示的文字")]
    public string DialogText;

    protected override void OnTriggerEnter2DAfter(Collider2D collision)
    {
        Dialog.ShowDialog(DialogText);
    }

    protected override void OnTriggerExit2DAfter(Collider2D collision)
    {
        Dialog.CloseDialog();
    }

    protected override void PlayerInteractive()
    {
        SceneManager.LoadScene(NextSceneName);
    }

   
}
