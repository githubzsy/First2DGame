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
    /// 提示文本的Panel
    /// </summary>
    [Header("提示文本的Panel")]
    public GameObject Dialog;

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

    private Dialog _dialog;
    void Start()
    {
        //设置要切换的场景
        Dialog.GetComponent<SwitchScene>().SetNextSceneName(NextSceneName);
        _dialog = Dialog.GetComponent<Dialog>();
    }

    // Update is called once per frame
    void Update()
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
            _dialog.ShowDialog(DialogText);
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
            _dialog.CloseDialog();
        }
    }
}
