using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 门的主函数
/// </summary>
public class Door : MonoBehaviour
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

    /// <summary>
    /// 显示对话框的脚本
    /// </summary>
    public Dialog Dialog;

    /// <summary>
    /// 切换场景的脚本
    /// </summary>
    public SwitchScene SwitchScene;
    void Awake()
    {
    }

    /// <summary>
    /// 2D触发器进入时
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Dialog.ShowDialog(DialogText);
            SwitchScene.SetNextSceneName(NextSceneName);
        }
    }

    /// <summary>
    /// 2D触发器离开时
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //玩家离开时关闭对话框
        if (collision.tag == "Player")
        {
            Dialog.CloseDialog();
            SwitchScene.ClearNextSceneName();
        }
    }
}
