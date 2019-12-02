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

    private void Awake()
    {
        _text = transform.Find("Text").GetComponent<Text>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
    }

    /// <summary>
    /// 启用Dialog并显示文字
    /// </summary>
    /// <param name="text">要显示的文字</param>
    internal void ShowDialog(string text)
    {
        if (string.IsNullOrWhiteSpace(text) == false)
        {
            gameObject.SetActive(true);
            if (_text == null)
            {

            }
            _text.text = text;
            //此处省略是因为Dialog默认启用动画就是ShowDialog
            //_animator.Play("ShowDialog");
        }
    }

    /// <summary>
    /// 关闭Dialog，后面动画播放完后会自动执行CloseDialogAnimCallBack
    /// </summary>
    internal void CloseDialog()
    {
        _text.text = null;
        _animator.SetTrigger("close");
    }

    /// <summary>
    /// 关闭Dialog动画执行完后的回调
    /// </summary>
    public void CloseDialogAnimCallBack()
    {
        gameObject.SetActive(false);
    }
}
