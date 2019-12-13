using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对话框主函数
/// </summary>
public class Dialog : MonoBehaviour
{
    private Text _text;

    private Animator _animator;

    private static Dialog _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _text = transform.Find("Text").GetComponent<Text>();
        _animator = GetComponent<Animator>();
        _instance = this;
    }

    /// <summary>
    /// 启用Dialog并显示文字
    /// </summary>
    /// <param name="text">要显示的文字</param>
    internal static void ShowDialog(string text)
    {
        if (string.IsNullOrWhiteSpace(text) == false)
        {
            UIManager.SetDialogActive();
            _instance._text.text = text;
        }
    }

    /// <summary>
    /// 启用Dialog并显示文字，并在一定时间后关闭
    /// </summary>
    /// <param name="text">要显示的文字</param>
    /// <param name="closeTime">关闭的时间</param>
    internal static void ShowDialog(string text,float closeTime)
    {
       ShowDialog(text);
       _instance.StartCoroutine(DelayToInvoke.DelayToInvokeDo(CloseDialog,closeTime));
    }

    /// <summary>
    /// 关闭Dialog，后面动画播放完后会自动执行CloseDialogAnimCallBack
    /// </summary>
    internal static void CloseDialog()
    {
        _instance._text.text = null;
        _instance._animator.SetTrigger("close");
    }

    /// <summary>
    /// 关闭Dialog动画执行完后的回调
    /// </summary>
    public void CloseDialogAnimCallBack()
    {
        _instance.gameObject.SetActive(false);
    }
}
